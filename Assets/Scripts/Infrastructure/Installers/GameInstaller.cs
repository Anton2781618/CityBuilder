using VContainer;
using VContainer.Unity;
using Domain;
using Application;
using MessagePipe;
using UnityEngine;
using Presentation.Views;

namespace Infrastructure.Installers
{
    /// <summary>
    /// Installer для DI: регистрация UseCase и событий.
    /// </summary>
    public class GameInstaller : LifetimeScope
    {
        //---------------------------------------------------------
        [Header("Настройки сетки")]
        [SerializeField] private GridCellView _cellPrefab;
        [SerializeField] private GridView _gridView;
        [SerializeField] private Vector2 _gridSize = new(10, 10);
        [SerializeField] private LayerMask _gridLayerMask;
        //---------------------------------------------------------
        //!---------------------------------------------------------

        protected override void Configure(IContainerBuilder builder)
        {
            //сетка
            builder.RegisterInstance(_gridLayerMask);
            builder.RegisterComponentInHierarchy<GridView>();
            builder.RegisterInstance(_cellPrefab);
            builder.RegisterInstance(new GridModel(_gridSize));

            // меню строительства
            builder.RegisterComponentInHierarchy<UIBuildMenuView>();
            
            // Экономика 
            builder.RegisterInstance(new PlayerEconomy(1000));
            
            // ввод
            builder.RegisterInstance(new InputSystem_Actions());

            // MessagePipe
            builder.RegisterMessagePipe();

            // UseCases с внедрением EventPublisher
            builder.Register<PlaceBuildingUseCase>(Lifetime.Singleton);
            builder.Register<MoveBuildingUseCase>(Lifetime.Singleton);
            builder.Register<RemoveBuildingUseCase>(Lifetime.Singleton);
            builder.Register<UpgradeBuildingUseCase>(Lifetime.Singleton);
        }
    }
}
