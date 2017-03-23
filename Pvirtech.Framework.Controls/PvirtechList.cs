using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Pvirtech.Framework.Controls
{
    public class PvirtechList:ListBox
    {
        private readonly Type _itemType;

		public static readonly DependencyProperty OrientationProperty;

		public static readonly DependencyProperty ItemContainerCommandProperty;

		public Orientation Orientation
		{
			get
			{
				return (Orientation)GetValue(OrientationProperty);
			}
			set
			{
				SetValue(OrientationProperty, value);
			}
		}

		public ICommand ItemContainerCommand
		{
			get
			{
				return (ICommand)GetValue(ItemContainerCommandProperty);
			}
			set
			{
				SetValue(ItemContainerCommandProperty, value);
			}
		}

        static PvirtechList()
		{
            OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(PvirtechList), new PropertyMetadata(Orientation.Horizontal));
            ItemContainerCommandProperty = DependencyProperty.Register("ItemContainerCommand", typeof(ICommand), typeof(PvirtechList), new PropertyMetadata(null));
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PvirtechList), new FrameworkPropertyMetadata(typeof(PvirtechList)));
		}

		public PvirtechList()
		{
            //var custermAttribute =typeof(PvirtechList).GetCustomAttributes(typeof(ContainerItemAttribute),true);
            //if (custermAttribute != null)
            //{
            //    _itemType = custermAttribute..ItemType;
            //}
           
        }
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new StoryBoardContainerItem();
        }


    }
}
