/**
 * FinalizeOwareMatch Cloud Code Script
 * 
 * This script calculates ELO ratings for players after an Oware match.
 * It fetches current ratings from Cloud Save, calculates new ratings,
 * updates Cloud Save, and submits the new ratings to the leaderboard.
 * 
 * Default rating: 1200
 * K-factor: 32 (standard for most chess-like games)
 */

const { CloudSaveModule } = require("@unity-services/cloud-save-1.3");
const { LeaderboardsModule } = require("@unity-services/leaderboards-1.0");

const DEFAULT_RATING = 1200;
const K_FACTOR = 32;
const LEADERBOARD_ID = "oware-elo-ratings";

module.exports = async ({ params, context }) => {
    const { winnerId, loserId, isDraw } = params;
    
    try {
        // Validate input
        if (!winnerId || !loserId) {
            throw new Error("Both winnerId and loserId are required");
        }
        
        if (winnerId === loserId) {
            throw new Error("Winner and loser cannot be the same player");
        }
        
        // Fetch current ratings from Cloud Save
        const winnerRating = await getPlayerRating(winnerId);
        const loserRating = await getPlayerRating(loserId);
        
        // Calculate expected scores using ELO formula
        const winnerExpected = calculateExpectedScore(winnerRating, loserRating);
        const loserExpected = calculateExpectedScore(loserRating, winnerRating);
        
        // Determine actual scores based on match result
        let winnerActual, loserActual;
        if (isDraw) {
            winnerActual = 0.5;
            loserActual = 0.5;
        } else {
            winnerActual = 1.0;
            loserActual = 0.0;
        }
        
        // Calculate new ratings
        const winnerNewRating = Math.round(winnerRating + K_FACTOR * (winnerActual - winnerExpected));
        const loserNewRating = Math.round(loserRating + K_FACTOR * (loserActual - loserExpected));
        
        // Calculate rating changes
        const winnerRatingChange = winnerNewRating - winnerRating;
        const loserRatingChange = loserNewRating - loserRating;
        
        // Update ratings in Cloud Save
        await updatePlayerRating(winnerId, winnerNewRating);
        await updatePlayerRating(loserId, loserNewRating);
        
        // Submit new ratings to leaderboard
        await submitToLeaderboard(winnerId, winnerNewRating);
        await submitToLeaderboard(loserId, loserNewRating);
        
        // Return the result
        return {
            WinnerId: winnerId,
            LoserId: loserId,
            WinnerOldRating: winnerRating,
            LoserOldRating: loserRating,
            WinnerNewRating: winnerNewRating,
            LoserNewRating: loserNewRating,
            WinnerRatingChange: winnerRatingChange,
            LoserRatingChange: loserRatingChange,
            IsDraw: isDraw || false
        };
        
    } catch (error) {
        console.error("Error in FinalizeOwareMatch:", error);
        throw error;
    }
};

/**
 * Get a player's current ELO rating from Cloud Save.
 * Returns DEFAULT_RATING if the player has no saved rating.
 */
async function getPlayerRating(playerId) {
    try {
        const result = await CloudSaveModule.getItems(playerId, ["eloRating"]);
        
        if (result && result.eloRating) {
            return parseInt(result.eloRating);
        }
        
        return DEFAULT_RATING;
    } catch (error) {
        console.log(`No rating found for player ${playerId}, using default: ${DEFAULT_RATING}`);
        return DEFAULT_RATING;
    }
}

/**
 * Update a player's ELO rating in Cloud Save.
 */
async function updatePlayerRating(playerId, newRating) {
    try {
        await CloudSaveModule.setItem(playerId, "eloRating", newRating);
        console.log(`Updated rating for player ${playerId}: ${newRating}`);
    } catch (error) {
        console.error(`Failed to update rating for player ${playerId}:`, error);
        throw error;
    }
}

/**
 * Submit a player's ELO rating to the leaderboard.
 */
async function submitToLeaderboard(playerId, rating) {
    try {
        await LeaderboardsModule.addPlayerScore(playerId, LEADERBOARD_ID, rating);
        console.log(`Submitted leaderboard score for player ${playerId}: ${rating}`);
    } catch (error) {
        console.error(`Failed to submit leaderboard score for player ${playerId}:`, error);
        // Don't throw - leaderboard submission is not critical
    }
}

/**
 * Calculate the expected score for a player using the ELO formula.
 * 
 * Formula: E = 1 / (1 + 10^((opponentRating - playerRating) / 400))
 */
function calculateExpectedScore(playerRating, opponentRating) {
    const exponent = (opponentRating - playerRating) / 400;
    return 1 / (1 + Math.pow(10, exponent));
}
