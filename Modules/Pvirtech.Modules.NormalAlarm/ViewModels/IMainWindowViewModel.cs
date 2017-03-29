using Pvirtech.Framework.Domain;
using System.ComponentModel;

namespace Pvirtech.Modules.NormalAlarm.ViewModels
{
    public interface IMainWindowViewModel :IHeaderInfoProvider<string>
    {
         ICollectionView   Users { get; }
    }
}
