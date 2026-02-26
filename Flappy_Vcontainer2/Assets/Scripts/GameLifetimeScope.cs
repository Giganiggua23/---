using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        Debug.Log("GameLifetimeScope.Configure() started");

        builder.RegisterComponentInHierarchy<ScoreManager>();
        Debug.Log("ScoreManager registered");

        builder.RegisterComponentInHierarchy<PlayerController>();
        Debug.Log("PlayerController registered");

        builder.RegisterComponentInHierarchy<ObjManager>();
        Debug.Log("ObjManager registered");

    }

    private void Start()
    {
        Debug.Log("Scope started working");
    }
}