using Godot;

public partial class NotificationWindow : SettingsWindow
{

    [Export] public Control JCloudToggleContainer { get; set; }
    [Export] public Control RealJCloudToggleContainer { get; set; }
    private CheckButton _jCloudToggle;
    private CheckButton _realJCloudToggle;
    private float _jCloudToggleValue = 0f;
    private Tween _jCloudTween;
    private Control _jCloudToggleProgressBarMask;
    public override void _Ready()
    {
        _jCloudToggle = JCloudToggleContainer.FindChild("CheckButton", true) as CheckButton;
        _jCloudToggleProgressBarMask = JCloudToggleContainer.FindChild("ToggleProgressMask", true) as Control;
        _jCloudToggle.GuiInput += OnJCloudToggleInput;

        _realJCloudToggle = RealJCloudToggleContainer.FindChild("CheckButton", true) as CheckButton;
        _realJCloudToggle.GuiInput += OnRealJCloudToggleInput;
    }

    public override void SetupWindow()
    {
        // Setup specific to the notification window
    }

    public override void LoadSettings()
    {
        // Load settings specific to the notification window
    }

    public override void SaveSettings()
    {
        // Save settings specific to the notification window
    }

    public override void Cleanup()
    {
        // Cleanup specific to the notification window
    }

    private void OnRealJCloudToggleInput(InputEvent @event)
    {
        if (@event.IsActionPressed("select"))
        {
            GD.Print("Real JCloud Toggled");
        }
    }

    private void OnJCloudToggleInput(InputEvent @event)
    {
        if (@event.IsActionPressed("select"))
        {
            _jCloudToggleValue += 1f;

            _jCloudToggleValue = Mathf.Min(_jCloudToggleValue, 10f);

            _jCloudTween?.Kill();

            if (_jCloudToggleValue >= 10f)
            {
                _jCloudToggle.ButtonPressed = true;
                GD.Print("JCloud Toggle Activated!");
                RealJCloudToggleContainer.Visible = true;
                // Optional: Reset value after activation
                // _jCloudToggleValue = 0f;
                return;
            }

            _jCloudTween = CreateTween();
            _jCloudTween.TweenMethod(
                Callable.From<float>(UpdateJCloudValue),
                _jCloudToggleValue,
                0f,
                _jCloudToggleValue / 2 // Duration in seconds - adjust for desired decay speed
            );
        }
    }

    private void UpdateJCloudValue(float value)
    {
        _jCloudToggleValue = value;

        _jCloudToggleProgressBarMask.Size = new Vector2(
            Mathf.Lerp(0f, this.Size.X, _jCloudToggleValue / 10f), // Assuming 100 is the max width of the mask
            _jCloudToggleProgressBarMask.Size.Y
        );

        // Optional: Update UI to show current value
        // someProgressBar.Value = _jCloudToggleValue;

        // Optional: Turn off toggle when value gets too low
        if (_jCloudToggleValue <= 0f && _jCloudToggle.ButtonPressed)
        {
            _jCloudToggle.ButtonPressed = false;
            GD.Print("JCloud Toggle Deactivated");
        }
    }
}