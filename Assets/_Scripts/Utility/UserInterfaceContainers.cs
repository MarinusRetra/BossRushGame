using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BossRush.Utility
{
    #region Components

    /// <summary>
    /// Container for text-based UI elements using TextMeshPro.
    /// </summary>
    [System.Serializable]
    public struct Label
    {
        /// <summary>The text content to display</summary>
        public string Text;
        /// <summary>Reference to the TextMeshPro component</summary>
        public TextMeshProUGUI TextComponent;
        /// <summary>Color of the text</summary>
        public Color TextColor;
        /// <summary>Font to be used for the text</summary>
        public Font Font;
    }

    /// <summary>
    /// Container for background panel UI elements.
    /// </summary>
    [System.Serializable]
    public struct Panel
    {
        /// <summary>Reference to the background Image component</summary>
        public Image Background;
        /// <summary>Color of the background</summary>
        public Color BackgroundColor;
        /// <summary>Dimensions of the panel</summary>
        public Vector2 Size;
    }

    /// <summary>
    /// Container for tooltip UI elements combining a panel and label.
    /// </summary>
    [System.Serializable]
    public struct Tooltip
    {
        /// <summary>Background panel for the tooltip</summary>
        public Panel Background;
        /// <summary>Text content of the tooltip</summary>
        public Label Text;
    }

    /// <summary>
    /// Container for progress bar UI elements with optional text display.
    /// </summary>
    [System.Serializable]
    public struct ProgressBar
    {
        /// <summary>Slider component for the progress bar</summary>
        public Slider Bar;
        /// <summary>Optional text to display progress value</summary>
        public Label ProgressText;
    }

    /// <summary>
    /// Container for input field UI elements using TextMeshPro.
    /// </summary>
    [System.Serializable]
    public struct InputField
    {
        /// <summary>Reference to the TextMeshPro input field component</summary>
        public TMP_InputField InputComponent;
    }

    #endregion

    #region Menus

    [System.Serializable]
    public struct MainMenu
    {
        public Panel Background;
        public Label Title;
        public Label Version;

        [Space]
        public InputField InputField;
        public InputField PlayerNameField;

        [Space]
        public Button[] Buttons;
    }

    #endregion

    // Sample class to showcase how this would work
    public class UserInterfaceContainers : MonoBehaviour
    {
        public MainMenu MainMenu;

        // We utilize the OnValidate method to simplify setting text and/or colors
        private void OnValidate()
        {
            // Change the main menu's title attributes, such as the text and color
            if (MainMenu.Title.TextComponent != null)
            {
                MainMenu.Title.TextComponent.text = MainMenu.Title.Text;
                MainMenu.Title.TextComponent.color = MainMenu.Title.TextColor;
                MainMenu.Title.TextComponent.alignment = TextAlignmentOptions.Center;
            }
        }
    }
}