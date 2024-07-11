using UnityEngine;

public class UIManager : MonoBehaviour
{
    private UIPresenter presenter;
    private UIModel model;
    private UIView view;

    private void Awake()
    {
        model = new UIModel();
        view = GetComponent<UIView>();
        presenter = new UIPresenter(view, model);
    }

    private void OnDestroy()
    {
        presenter.Cleanup();
    }
}
