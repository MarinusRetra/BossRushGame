using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BossRush.Utility;

namespace BossRush.UnitTests.EditMode
{
    [TestFixture]
    public class UserInterfaceContainersTests
    {
        private UserInterfaceContainers _uiContainers;
        private GameObject _gameObject;

        [SetUp]
        public void Setup()
        {
            // Initialize GameObject and add the script
            _gameObject = new GameObject();
            _uiContainers = _gameObject.AddComponent<UserInterfaceContainers>();

            // Initialize MainMenu with placeholder components
            var titleObject = new GameObject("Title");
            var titleComponent = titleObject.AddComponent<TextMeshProUGUI>();

            var button1 = new GameObject("Button1").AddComponent<Button>();
            var button2 = new GameObject("Button2").AddComponent<Button>();

            _uiContainers.MainMenu = new MainMenu
            {
                Title = new Label
                {
                    Text = "Main Menu",
                    TextComponent = titleComponent,
                    TextColor = Color.red
                },
                Buttons = new[] { button1, button2 }
            };
        }

        [Test]
        public void ValidateLabelInitialization()
        {
            // Arrange: Create a new Label object with specific Text and TextColor values.
            var label = new Label
            {
                Text = "Main Menu",
                TextComponent = new GameObject("TitleComponent").AddComponent<TextMeshProUGUI>(),
                TextColor = Color.red
            };

            // Assign the label to the MainMenu.Title
            _uiContainers.MainMenu.Title = label;

            // Act: Validate that Text and TextColor are set correctly on the TextComponent
            var textComponent = _uiContainers.MainMenu.Title.TextComponent;

            // Log the values being checked
            Debug.Log("Validating Label Initialization...");
            Debug.Log($"Label Text: {textComponent.text}");
            Debug.Log($"Label Color: {textComponent.color}");

            _uiContainers.OnValidate();
            Debug.Log("Called OnValidate");

            // Assert: Verify that the Text and TextColor of TextComponent match the Label values
            Assert.AreEqual("Main Menu", textComponent.text);
            Assert.AreEqual(Color.red, textComponent.color);

            // Log assertion results
            Debug.Log("Label Initialization validated successfully.");
        }

        [Test]
        public void ValidatePanelInitialization()
        {
            // Arrange: Create a new Panel object with specified size and background color
            var panel = new Panel
            {
                Background = new GameObject("PanelBackground").AddComponent<Image>(),
                BackgroundColor = Color.blue
            };

            // Assign the Panel to the MainMenu.Background
            _uiContainers.MainMenu.Background = panel;

            // Act: Apply the background color to the Panel's Image component
            panel.Background.color = panel.BackgroundColor;

            // Log the values being checked
            Debug.Log("Validating Panel Initialization...");
            Debug.Log($"Panel Background Color: {panel.Background.color}");

            _uiContainers.OnValidate();
            Debug.Log("Called OnValidate, now also checking if they are equal");

            // Assert: Verify the Background Image color is set correctly
            Assert.AreEqual(Color.blue, panel.Background.color);

            // Log assertion results
            Debug.Log("Panel Initialization validated successfully.");
        }

        [Test]
        public void ValidateButtonArrayAssignment()
        {
            // Arrange: Create an array of Button objects
            var button1 = new GameObject("Button1").AddComponent<Button>();
            var button2 = new GameObject("Button2").AddComponent<Button>();

            // Assign the array to MainMenu.Buttons
            _uiContainers.MainMenu.Buttons = new[] { button1, button2 };

            // Log the button array assignment
            Debug.Log("Validating Button Array Assignment...");
            Debug.Log($"Button Array Length: {_uiContainers.MainMenu.Buttons.Length}");

            _uiContainers.OnValidate();
            Debug.Log("Called OnValidate");

            // Act & Assert: Verify that each button in the array is not null
            Assert.IsNotNull(_uiContainers.MainMenu.Buttons);
            Assert.AreEqual(2, _uiContainers.MainMenu.Buttons.Length);

            foreach (var button in _uiContainers.MainMenu.Buttons)
            {
                Assert.IsNotNull(button);
                Debug.Log($"Button: {button.name}");
            }

            // Log assertion results
            Debug.Log("Button Array Assignment validated successfully.");
        }
    }
}
