using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Pvirtech.Framework.Controls
{
	public class StoryBoardContainerPanle : Decorator
	{
		public static readonly DependencyProperty StrokBrushProperty = DependencyProperty.Register("StrokBrush", typeof(Brush), typeof(StoryBoardContainerPanle), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

		public static readonly DependencyProperty StrokThicknessProperty = DependencyProperty.Register("StrokThickness", typeof(double), typeof(StoryBoardContainerPanle), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

		public static readonly DependencyProperty EllipseBrushProperty = DependencyProperty.Register("EllipseBrush", typeof(Brush), typeof(StoryBoardContainerPanle), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

		public static readonly DependencyProperty SpliterPostionProperty = DependencyProperty.Register("SpliterPostion", typeof(int), typeof(StoryBoardContainerPanle), new FrameworkPropertyMetadata(15, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

		public static readonly DependencyProperty EllipseSizeProperty = DependencyProperty.Register("EllipseSize", typeof(int), typeof(StoryBoardContainerPanle), new FrameworkPropertyMetadata(5, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

		public Brush StrokBrush
		{
			get
			{
				return (Brush)base.GetValue(StoryBoardContainerPanle.StrokBrushProperty);
			}
			set
			{
				base.SetValue(StoryBoardContainerPanle.StrokBrushProperty, value);
			}
		}

		public double StrokThickness
		{
			get
			{
				return (double)base.GetValue(StoryBoardContainerPanle.StrokThicknessProperty);
			}
			set
			{
				base.SetValue(StoryBoardContainerPanle.StrokThicknessProperty, value);
			}
		}

		public Brush EllipseBrush
		{
			get
			{
				return (Brush)base.GetValue(StoryBoardContainerPanle.EllipseBrushProperty);
			}
			set
			{
				base.SetValue(StoryBoardContainerPanle.EllipseBrushProperty, value);
			}
		}

		public int SpliterPostion
		{
			get
			{
				return (int)base.GetValue(StoryBoardContainerPanle.SpliterPostionProperty);
			}
			set
			{
				base.SetValue(StoryBoardContainerPanle.SpliterPostionProperty, value);
			}
		}

		public int EllipseSize
		{
			get
			{
				return (int)base.GetValue(StoryBoardContainerPanle.EllipseSizeProperty);
			}
			set
			{
				base.SetValue(StoryBoardContainerPanle.EllipseSizeProperty, value);
			}
		}

		protected override void OnRender(DrawingContext dc)
		{
			Pen pen = new Pen(this.StrokBrush, this.StrokThickness);
			dc.DrawLine(pen, new Point((double)this.SpliterPostion, 0.0), new Point((double)this.SpliterPostion, base.ActualHeight - (double)(this.EllipseSize * 2)));
			dc.DrawEllipse(this.EllipseBrush, pen, new Point((double)this.SpliterPostion, base.ActualHeight - (double)this.EllipseSize), (double)this.EllipseSize, (double)this.EllipseSize);
			base.OnRender(dc);
		}
	}
}
