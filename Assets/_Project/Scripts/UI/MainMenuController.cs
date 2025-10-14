using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class MainMenuController : MonoBehaviour {
    UIDocument _ui;
    Button _quickPlayBtn, _hostBtn, _joinBtn, _exitBtn;
    TextField _joinCodeField;
    Label _status;

    void Awake() {
        _ui = GetComponent<UIDocument>();
        var root = _ui.rootVisualElement;

        _quickPlayBtn = root.Q<Button>("QuickPlayButton");
        _hostBtn = root.Q<Button>("HostButton");
        _joinBtn = root.Q<Button>("JoinButton");
        _joinCodeField = root.Q<TextField>("JoinCodeField");
        _exitBtn = root.Q<Button>("ExitButton");
        _status = root.Q<Label>("StatusLabel");

        if (_exitBtn != null)
            _exitBtn.clicked += () => Application.Quit();

        if (_quickPlayBtn != null) _quickPlayBtn.clicked += async () => await QuickPlayFlow();
        if (_hostBtn != null) _hostBtn.clicked += async () => await HostFlow();
        if (_joinBtn != null) _joinBtn.clicked += async () => await JoinFlow();
    }

    async Task QuickPlayFlow() {
        _status.text = "Searching for available session...";
        try {
            await SessionsService.I.QuickJoinAsync();
            _status.text = "Joined session!";
        } catch {
            _status.text = "No sessions found. Creating new one...";
            await SessionsService.I.HostSessionAsync();
            _status.text = "New session created.";
        }
        SceneManager.LoadScene("MatchLobby");
    }

    async Task HostFlow() {
        _status.text = "Creating session...";
        await SessionsService.I.HostSessionAsync();
        _status.text = "Hosting session...";
        SceneManager.LoadScene("MatchLobby");
    }

    async Task JoinFlow() {
        string code = _joinCodeField?.text?.Trim();
        if (string.IsNullOrEmpty(code)) {
            _status.text = "Please enter a join code.";
            return;
        }

        _status.text = "Joining session...";
        try {
            await SessionsService.I.JoinSessionAsync(code);
            _status.text = $"Joined session {code}.";
            SceneManager.LoadScene("MatchLobby");
        } catch (System.Exception e) {
            _status.text = $"Join failed: {e.Message}";
        }
    }
}
