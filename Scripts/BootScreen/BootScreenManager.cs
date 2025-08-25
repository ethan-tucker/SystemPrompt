using Godot;
using System;
using System.Threading.Tasks;

public partial class BootScreenManager : Control
{
    [Export] public LineEdit PasswordInput { get; set; }
    private bool _hasEnteredPassword = false;
    private bool _checkingAnswer = false;
    public override void _Ready()
    {
        PasswordInput.TextSubmitted += OnPasswordSubmitted;
    }

    private async void OnPasswordSubmitted(string password)
    {
        if (_checkingAnswer || PasswordInput.Text == string.Empty) return;
        _checkingAnswer = true;
        PasswordInput.Editable = false;

        if (_hasEnteredPassword) {

        }
        else
        {
            _hasEnteredPassword = true;
            ShakeTextBox(PasswordInput, 3, .15f, 1, () =>
            {
                PasswordInput.Text = string.Empty;
                _checkingAnswer = false;
                PasswordInput.Editable = true;
            });
        }
    }
    public void ShakeTextBox(LineEdit lineEdit, float intensity = 10f, float duration = 0.5f, int shakeCount = 4, System.Action onComplete = null)
    {
        Vector2 originalPosition = lineEdit.Position;
        Tween tween = CreateTween();

        float shakeTime = duration / (shakeCount * 2);

        for (int i = 0; i < shakeCount; i++)
        {
            // Shake left
            tween.TweenProperty(lineEdit, "position:x", originalPosition.X - intensity, shakeTime);
            // Shake right
            tween.TweenProperty(lineEdit, "position:x", originalPosition.X + intensity, shakeTime);
        }

        // Return to center
        tween.TweenProperty(lineEdit, "position", originalPosition, shakeTime);

        if(onComplete != null) tween.TweenCallback(Callable.From(onComplete));
    }
}
