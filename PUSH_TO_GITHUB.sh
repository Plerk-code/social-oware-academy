#!/bin/bash

# Script to push Social Oware Academy docs to GitHub
# Replace YOUR_USERNAME with your actual GitHub username

echo "=========================================="
echo "Pushing Social Oware Academy to GitHub"
echo "=========================================="
echo ""

# Check if we're in the right directory
if [ ! -d ".git" ]; then
    echo "Error: Not in a Git repository. Please run this from:"
    echo "/Users/benjaminhinson/Unity Projects/Oware/My project"
    exit 1
fi

# Get GitHub username
echo "Enter your GitHub username:"
read GITHUB_USERNAME

if [ -z "$GITHUB_USERNAME" ]; then
    echo "Error: GitHub username cannot be empty"
    exit 1
fi

# Construct repository URL
REPO_URL="https://github.com/$GITHUB_USERNAME/social-oware-academy.git"

echo ""
echo "Repository URL: $REPO_URL"
echo ""
echo "Make sure you've created the repository 'social-oware-academy' on GitHub first!"
echo "Go to: https://github.com/new"
echo ""
echo "Press Enter to continue, or Ctrl+C to cancel..."
read

# Add remote (ignore error if already exists)
git remote add origin "$REPO_URL" 2>/dev/null || git remote set-url origin "$REPO_URL"

# Rename branch to main (if needed)
git branch -M main

# Push to GitHub
echo ""
echo "Pushing to GitHub..."
git push -u origin main

if [ $? -eq 0 ]; then
    echo ""
    echo "=========================================="
    echo "✅ SUCCESS! Documentation uploaded!"
    echo "=========================================="
    echo ""
    echo "View your repository at:"
    echo "https://github.com/$GITHUB_USERNAME/social-oware-academy"
    echo ""
else
    echo ""
    echo "❌ Push failed. Common issues:"
    echo "1. Repository doesn't exist on GitHub - create it first at https://github.com/new"
    echo "2. Authentication failed - you may need to use a Personal Access Token"
    echo "   See: https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/creating-a-personal-access-token"
    echo "3. Wrong username - double-check your GitHub username"
    exit 1
fi
