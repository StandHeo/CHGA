using System;
using System.Windows;
using System.Windows.Media;
 

namespace Pvirtech.Framework.Controls
{
	[ContainerItem(typeof(StoryBoardContainerItem))]
	public class StoryBoardContainer : PvirtechList
	{
		public static readonly DependencyProperty SpliterPostionProperty;

		public static readonly DependencyProperty EllipseSizeProperty;

		public static readonly DependencyProperty EllipseBrushProperty;

		public static readonly DependencyProperty RefrenceLineBrushProperty;

		public static readonly DependencyProperty RefrenceLineThicknessProperty;

		public int SpliterPostion
		{
			get
			{
				return (int)GetValue(SpliterPostionProperty);
			}
			set
			{
				SetValue(SpliterPostionProperty, value);
			}
		}

		public int EllipseSize
		{
			get
			{
				return (int)GetValue(EllipseSizeProperty);
			}
			set
			{
				SetValue(EllipseSizeProperty, value);
			}
		}

		public Brush EllipseBrush
		{
			get
			{
				return (Brush)GetValue(EllipseBrushProperty);
			}
			set
			{
				SetValue(EllipseBrushProperty, value);
			}
		}

		public Brush RefrenceLineBrush
		{
			get
			{
				return (Brush)GetValue(RefrenceLineBrushProperty);
			}
			set
			{
				SetValue(RefrenceLineBrushProperty, value);
			}
		}

		public int RefrenceLineThickness
		{
			get
			{
				return (int)GetValue(RefrenceLineThicknessProperty);
			}
			set
			{
				base.SetValue(RefrenceLineThicknessProperty, value);
			}
		}

		static StoryBoardContainer()
		{
			SpliterPostionProperty = DependencyProperty.Register("SpliterPostion", typeof(int), typeof(StoryBoardContainer), new PropertyMetadata(15));
			EllipseSizeProperty = DependencyProperty.Register("EllipseSize", typeof(int), typeof(StoryBoardContainer), new PropertyMetadata(5));
			EllipseBrushProperty = DependencyProperty.Register("EllipseBrush", typeof(Brush), typeof(StoryBoardContainer), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
			RefrenceLineBrushProperty = DependencyProperty.Register("RefrenceLineBrush", typeof(Brush), typeof(StoryBoardContainer), new PropertyMetadata(Brushes.Transparent));
			RefrenceLineThicknessProperty = DependencyProperty.Register("RefrenceLineThickness", typeof(int), typeof(StoryBoardContainer), new PropertyMetadata(2));
			DefaultStyleKeyProperty.OverrideMetadata(typeof(StoryBoardContainer), new FrameworkPropertyMetadata(typeof(StoryBoardContainer)));
		}
	}
}
