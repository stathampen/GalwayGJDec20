using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menus
{
	public class MainMenuControl : MonoBehaviour
	{
		[SerializeField] private GameObject instructionsPanel;
		[SerializeField] private GameObject instructionsButton;
		[SerializeField] private float transitionTime;
		[SerializeField] private LevelController levelController;
		[SerializeField] private GameObject mainMenuFab;
		[SerializeField] private GameObject buttonsPanel;
		[SerializeField] private GameObject titlePanel;
		[SerializeField] private GameObject playerUIPanel;
		[SerializeField] private GameObject gameSceneObject;

		private Image _panelImage;
		private TextMeshProUGUI _panelText;
		private Image _buttonImage;
		private TextMeshProUGUI _buttonText;

		private Color _originalInstructionsPanelColour;
		private float _originalInstructionsPanelTransparencyLimit;
		private Color _originalInstructionsButtonColour;
		private Color _originalInstructionsPanelTextColour;
		private Color _originalInstructionsButtonTextColour;

		private float _currentTime;
		private bool _transitioningToEnabledPanel;

		public void PlayButtonClicked()
		{
			instructionsPanel.SetActive(true);
			_panelImage = instructionsPanel.GetComponent<Image>();
			_panelText = instructionsPanel.GetComponentInChildren<TextMeshProUGUI>();
			_buttonImage = instructionsButton.GetComponent<Image>();
			_buttonText = instructionsButton.GetComponentInChildren<TextMeshProUGUI>();

			_originalInstructionsPanelColour = _panelImage.color;
			_originalInstructionsButtonColour = _buttonImage.color;
			_originalInstructionsPanelTextColour = _panelText.color;
			_originalInstructionsButtonTextColour = _buttonText.color;
			_originalInstructionsPanelTransparencyLimit = _panelImage.color.a;

			_panelImage.color = new Color(_originalInstructionsPanelColour.r,
				_originalInstructionsPanelColour.g,
				_originalInstructionsPanelColour.b,
				0);

			_buttonImage.color = new Color(_originalInstructionsButtonColour.r,
				_originalInstructionsButtonColour.g,
				_originalInstructionsButtonColour.b,
				0);

			_panelText.color = new Color(_originalInstructionsPanelTextColour.r,
				_originalInstructionsPanelTextColour.g,
				_originalInstructionsPanelTextColour.b,
				0);

			_buttonText.color = new Color(_originalInstructionsButtonTextColour.r,
				_originalInstructionsButtonTextColour.g,
				_originalInstructionsButtonTextColour.b,
				0);

			_transitioningToEnabledPanel = true;
		}

		public void LoadFirstLevel()
		{
			instructionsPanel.SetActive(false);
			mainMenuFab.SetActive(false);
			buttonsPanel.SetActive(false);
			titlePanel.SetActive(false);
			playerUIPanel.SetActive(true);
			gameSceneObject.SetActive(true);
			levelController.LoadLevel();
		}

		void Update()
		{
			if (_transitioningToEnabledPanel && _currentTime <= transitionTime)
			{
				_currentTime += Time.deltaTime;
				var panelTransparency =
					_originalInstructionsPanelTransparencyLimit * (_currentTime / transitionTime);
				var alpha = (_currentTime / transitionTime);

				_panelImage.color = new Color(_originalInstructionsPanelColour.r,
					_originalInstructionsPanelColour.g,
					_originalInstructionsPanelColour.b,
					panelTransparency);

				_buttonImage.color = new Color(_originalInstructionsButtonColour.r,
					_originalInstructionsButtonColour.g,
					_originalInstructionsButtonColour.b,
					alpha);

				_panelText.color = new Color(_originalInstructionsPanelTextColour.r,
					_originalInstructionsPanelTextColour.g,
					_originalInstructionsPanelTextColour.b,
					alpha);

				_buttonText.color = new Color(_originalInstructionsButtonTextColour.r,
					_originalInstructionsButtonTextColour.g,
					_originalInstructionsButtonTextColour.b,
					alpha);
			}
		}
	}
}
