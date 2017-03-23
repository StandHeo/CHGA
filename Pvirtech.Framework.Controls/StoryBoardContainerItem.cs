using System;
using System.Windows;
using System.Windows.Controls;

namespace Pvirtech.Framework.Controls
{
	public class StoryBoardContainerItem : ContentControl
	{
		public static readonly DependencyProperty FiredTimeTemplateProperty;

		public FrameworkElement FiredTimeTemplate
		{
			get
			{
				return (FrameworkElement)GetValue(FiredTimeTemplateProperty);
			}
			set
			{
				SetValue(FiredTimeTemplateProperty, value);
			}
		}

		static StoryBoardContainerItem()
		{
			FiredTimeTemplateProperty = DependencyProperty.Register("FiredTimeTemplate", typeof(FrameworkElement), typeof(StoryBoardContainerItem), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
			DefaultStyleKeyProperty.OverrideMetadata(typeof(StoryBoardContainerItem), new FrameworkPropertyMetadata(typeof(StoryBoardContainerItem)));
		}
	}
}
