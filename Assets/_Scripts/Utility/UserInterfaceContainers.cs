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

    /// <summary>
    /// Container for the main menu UI elements and layout.
    /// </summary>
    [System.Serializable]
    public struct MainMenu
    {
        /// <summary>
        /// The background panel of the main menu.
        /// </summary>
        [Header("Core Components")]
        [Tooltip("Main background panel container")]
        public Panel Background;

        /// <summary>
        /// The title text display of the main menu.
        /// </summary>
        [Tooltip("Title text displayed at the top of the menu")]
        public Label Title;

        /// <summary>
        /// The version text display of the game.
        /// </summary>
        [Tooltip("Version number text display")]
        public Label Version;

        /// <summary>
        /// Server purpose input field for the menu to join other servers.
        /// </summary>
        [Header("Input Fields")]
        [Tooltip("Server input field for joining servers")]
        public InputField ServerInputField;

        /// <summary>
        /// Input field specifically for the player's name.
        /// </summary>
        [Tooltip("Input field for entering the player's name")]
        public InputField PlayerNameField;

        /// <summary>
        /// Array of interactive buttons in the main menu.
        /// </summary>
        [Header("Navigation")]
        [Tooltip("Collection of menu buttons (Play, Options, Quit, etc.)")]
        public Button[] Buttons;
    }

    #endregion

    // Sample class to showcase how this would work
    public class UserInterfaceContainers : MonoBehaviour
    {
        public MainMenu MainMenu;

        // We utilize the OnValidate method to simplify setting text and/or colors
        public void OnValidate()
        {
            // Change the main menu's title attributes, such as the text, alignment and color
            if (MainMenu.Title.TextComponent != null)
            {
                MainMenu.Title.TextComponent.text = MainMenu.Title.Text;
                MainMenu.Title.TextComponent.color = MainMenu.Title.TextColor;
                MainMenu.Title.TextComponent.alignment = TextAlignmentOptions.Center;
            }
        }
    }
}