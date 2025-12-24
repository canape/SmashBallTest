using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ProjectScriptableObjectInstaller", menuName = "Installers/ProjectScriptableObjectInstaller")]
public class ProjectScriptableObjectInstaller : ScriptableObjectInstaller<ProjectScriptableObjectInstaller>
{
    [SerializeField] private DialogsData dialogsData;
    [SerializeField] private CourtsData courtsData;
    [SerializeField] private HeroesData heroesData;

    public override void InstallBindings()
    {
        Container.BindInstances(dialogsData);
        Container.BindInstances(courtsData);
        Container.BindInstances(heroesData);
    }
}