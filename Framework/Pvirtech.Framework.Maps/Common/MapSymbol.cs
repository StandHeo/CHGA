using ESRI.ArcGIS.Client.Symbols;
using System;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Pvirtech.Framework.Maps
{
	public class MapSymbol
	{
		public static SimpleFillSymbol GetDefaultSimpleFillSymbol()
		{
			return GetSimpleFillSymbol(Colors.Red, 1.0, 1.0, Colors.Red, 1.0);
		}

		public static SimpleLineSymbol GetSimpleLineSymbol(Color color, double width, SimpleLineSymbol.LineStyle lineStyle)
		{
			SolidColorBrush color2 = new SolidColorBrush(color);
			return new SimpleLineSymbol
			{
				Color = color2,
				Width = width,
				Style = lineStyle
			};
		}

		public static SimpleFillSymbol GetSimpleFillSymbol(Color borderColor, double borderOpacity, double borderThick, Color fillColor, double fillOpacity)
		{
			SolidColorBrush borderBrush = new SolidColorBrush(borderColor);
			SolidColorBrush fill = new SolidColorBrush(fillColor);
            var simplefillSymbol = new SimpleFillSymbol();
            borderBrush.Opacity = borderOpacity;
            fill.Opacity = fillOpacity;
            simplefillSymbol.BorderBrush = borderBrush;
            simplefillSymbol.BorderThickness = borderThick; 
            simplefillSymbol.Fill = fill;
            return simplefillSymbol;
   //         return new SimpleFillSymbol();
			//{
			//	BorderBrush = borderBrush,
			//	BorderBrush = 
			//	{
			//		Opacity = borderOpacity
			//	},
			//	BorderThickness = borderThick,
			//	Fill =new SolidColorBrush(fill,
			//	 = 
			//	{
			//		Opacity = fillOpacity
			//	} 
		}

		public static TextSymbol GetTextSymbol(string labelText, string fontName, double fontSize, Color fontColor)
		{
			TextSymbol result;
			if (string.IsNullOrWhiteSpace(labelText))
			{
				result = null;
			}
			else
			{
				if (string.IsNullOrWhiteSpace(fontName))
				{
					fontName = "Arial";
				}
				result = new TextSymbol
				{
					FontFamily = new FontFamily(fontName),
					FontSize = fontSize,
					Foreground = new SolidColorBrush(fontColor),
					Text = labelText,
					OffsetX = 0.0,
					OffsetY = 6.0
				};
			}
			return result;
		}

		public static SimpleMarkerSymbol GetSimpleMarkerSymbol(Color color, double size, int style)
		{
			return new SimpleMarkerSymbol
			{
				Color = new SolidColorBrush(color),
				Size = size,
				Style = GetSimpleMarkerStyleByInt(style)
			};
		}

		public static SimpleMarkerSymbol GetSimpleMarkerSymbol(Color color, double size, SimpleMarkerSymbol.SimpleMarkerStyle markerStyle)
		{
			return new SimpleMarkerSymbol
			{
				Color = new SolidColorBrush(color),
				Size = size,
				Style = markerStyle
			};
		}

		private static SimpleMarkerSymbol.SimpleMarkerStyle GetSimpleMarkerStyleByInt(int style)
		{
			SimpleMarkerSymbol.SimpleMarkerStyle result = SimpleMarkerSymbol.SimpleMarkerStyle.Circle;
			if (Enum.IsDefined(typeof(SimpleMarkerSymbol.SimpleMarkerStyle), style))
			{
				result = (SimpleMarkerSymbol.SimpleMarkerStyle)style;
			}
			return result;
		}

		public static PictureMarkerSymbol GetPictureMarkerSymbol(string pictureSource, double width, double height, double offsetX, double offsetY, double opacity)
		{
			ImageSource imageSource = new BitmapImage(new Uri(pictureSource, UriKind.Relative));
			return GetPictureMarkerSymbol(imageSource, width, height, offsetX, offsetY, opacity);
		}

		public static PictureMarkerSymbol GetPictureMarkerSymbol(ImageSource imageSource, double width, double height, double offsetX, double offsetY, double opacity)
		{
			return new PictureMarkerSymbol
			{
				Source = imageSource,
				Width = width,
				Height = height,
				OffsetX = offsetX,
				OffsetY = offsetY,
				Opacity = opacity
			};
		}

		public static MarkerSymbol GetCustomStrobeMarkerSymbolByNormal(double circleSize, Color colorCircle, Color colorCircleBorder, int durationSecond, int repeatBehavior)
		{
			string text = "<ControlTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'>\r\n                <Grid>\r\n                  <VisualStateManager.VisualStateGroups>\r\n                    <VisualStateGroup x:Name='CommonStates'>\r\n                      <VisualState x:Name='MouseOver'/>\r\n                      <VisualState x:Name='Normal'>\r\n                        <Storyboard RepeatBehavior ='3x'>\r\n                          <DoubleAnimation BeginTime='0' Storyboard.TargetName='ellipse' Storyboard.TargetProperty='(UIElement.RenderTransform).(ScaleTransform.ScaleX)' From='1' To='9' Duration='00:00:01' />\r\n                          <DoubleAnimation BeginTime='0' Storyboard.TargetName='ellipse' Storyboard.TargetProperty='(UIElement.RenderTransform).(ScaleTransform.ScaleY)' From='1' To='9' Duration='00:00:01' />\r\n                          <DoubleAnimation BeginTime='0' Storyboard.TargetName='ellipse' Storyboard.TargetProperty='(UIElement.Opacity)' From='1' To='0' Duration='00:00:01' />\r\n                        </Storyboard>\r\n                      </VisualState>\r\n                    </VisualStateGroup>\r\n                  </VisualStateManager.VisualStateGroups>\r\n                  <Ellipse Height='10' Width='10' Canvas.Left='-5' Canvas.Top='-5' RenderTransformOrigin='0.5,0.5' x:Name='ellipse' IsHitTestVisible='False'>\r\n                    <Ellipse.RenderTransform>\r\n                      <ScaleTransform />\r\n                    </Ellipse.RenderTransform>\r\n                    <Ellipse.Fill>\r\n                      <RadialGradientBrush>\r\n                        <GradientStop Color='#00FF0000' />\r\n                        <GradientStop Color='#FFFF0000' Offset='0.25' />\r\n                        <GradientStop Color='#00FF0000' Offset='0.5' />\r\n                        <GradientStop Color='#FFFF0000' Offset='0.75' />\r\n                        <GradientStop Color='#00FF0000' Offset='1' />\r\n                      </RadialGradientBrush>\r\n                    </Ellipse.Fill>            \r\n                  </Ellipse>\r\n                  <Ellipse Height='10' Width='10' Canvas.Left='-5' Canvas.Top='-5' Fill='#FFFF0000' x:Name='ellipse1' />\r\n                </Grid>\r\n              </ControlTemplate>";
			if (circleSize != 0.0)
			{
				text = text.Replace("9", circleSize.ToString());
			}
			if (durationSecond != 0)
			{
				string newValue = "00:00:" + durationSecond.ToString().PadLeft(2, '0');
				text = text.Replace("00:00:01", newValue);
			}
			text = text.Replace("#00FF0000", colorCircle.ToString());
			text = text.Replace("#FFFF0000", colorCircle.ToString());
			string newValue2 = "Forever";
			if (repeatBehavior != 0)
			{
				newValue2 = repeatBehavior.ToString() + "x";
			}
			text = text.Replace("3x", newValue2);
			MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(text));
			ControlTemplate controlTemplate = XamlReader.Load(stream) as ControlTemplate;
			return new MarkerSymbol
			{
				ControlTemplate = controlTemplate
			};
		}

		public static MarkerSymbol GetCustomStrobeMarkerSymbolByMouseOver(double circleSize, Color colorCircle, Color colorCircleBorder, int durationSecond)
		{
			string text = "<ControlTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'>\r\n            <Grid>\r\n              <VisualStateManager.VisualStateGroups>\r\n                <VisualStateGroup x:Name='CommonStates'>\r\n                  <VisualState x:Name='MouseOver'>\r\n                    <Storyboard RepeatBehavior ='Forever'>\r\n                      <DoubleAnimation BeginTime='0' Storyboard.TargetName='ellipse' Storyboard.TargetProperty='(UIElement.RenderTransform).(ScaleTransform.ScaleX)' From='1' To='9' Duration='00:00:01' />\r\n                      <DoubleAnimation BeginTime='0' Storyboard.TargetName='ellipse' Storyboard.TargetProperty='(UIElement.RenderTransform).(ScaleTransform.ScaleY)' From='1' To='9' Duration='00:00:01' />\r\n                      <DoubleAnimation BeginTime='0' Storyboard.TargetName='ellipse' Storyboard.TargetProperty='(UIElement.Opacity)' From='1' To='0' Duration='00:00:01' />\r\n                    </Storyboard>\r\n                  </VisualState>\r\n                  <VisualState x:Name='Normal'/>\r\n                </VisualStateGroup>\r\n              </VisualStateManager.VisualStateGroups>\r\n              <Ellipse Height='10' Width='10' Canvas.Left='-5' Canvas.Top='-5' RenderTransformOrigin='0.5,0.5' x:Name='ellipse' IsHitTestVisible='False'>\r\n                <Ellipse.RenderTransform>\r\n                  <ScaleTransform />\r\n                </Ellipse.RenderTransform>\r\n                <Ellipse.Fill>\r\n                  <RadialGradientBrush>\r\n                    <GradientStop Color='#00FF0000' />\r\n                    <GradientStop Color='#FFFF0000' Offset='0.25' />\r\n                    <GradientStop Color='#00FF0000' Offset='0.5' />\r\n                    <GradientStop Color='#FFFF0000' Offset='0.75' />\r\n                    <GradientStop Color='#00FF0000' Offset='1' />\r\n                  </RadialGradientBrush>\r\n                </Ellipse.Fill>            \r\n              </Ellipse>\r\n              <Ellipse Height='10' Width='10' Canvas.Left='-5' Canvas.Top='-5' Fill='#FFFF0000' x:Name='ellipse1' />\r\n            </Grid>\r\n          </ControlTemplate>";
			if (circleSize != 0.0)
			{
				text = text.Replace("9", circleSize.ToString());
			}
			if (durationSecond != 0)
			{
				string newValue = "00:00:" + durationSecond.ToString().PadLeft(2, '0');
				text = text.Replace("00:00:01", newValue);
			}
			text = text.Replace("#00FF0000", colorCircleBorder.ToString());
			text = text.Replace("#FFFF0000", colorCircle.ToString());
			Stream stream = new MemoryStream(Encoding.Default.GetBytes(text));
			ControlTemplate controlTemplate = XamlReader.Load(stream) as ControlTemplate;
			return new MarkerSymbol
			{
				ControlTemplate = controlTemplate
			};
		}

		public static FillSymbol GetCustomGlowHotFillSymbol(Color colorFill, Color colorGlow, int durationSecond)
		{
			string text = "<ControlTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'>\r\n            <Grid>\r\n                <VisualStateManager.VisualStateGroups>\r\n                <VisualStateGroup x:Name=\"CommonStates\">\r\n                    <VisualState x:Name=\"MouseOver\">\r\n                    <Storyboard>\r\n                        <DoubleAnimation BeginTime=\"0\" Storyboard.TargetName=\"Element\" Storyboard.TargetProperty=\"(Shape.Fill).(GradientBrush.GradientStops)[1].(GradientStop.Offset)\" To=\"1.5\" Duration=\"00:00:01\" />\r\n                    </Storyboard>\r\n                    </VisualState>\r\n                    <VisualState x:Name=\"Normal\">\r\n                    <Storyboard>\r\n                        <DoubleAnimation BeginTime=\"0\" Storyboard.TargetName=\"Element\" Storyboard.TargetProperty=\"(Shape.Fill).(GradientBrush.GradientStops)[1].(GradientStop.Offset)\" To=\"0\" Duration=\"00:00:01\" />\r\n                    </Storyboard>\r\n                    </VisualState>\r\n                </VisualStateGroup>\r\n                </VisualStateManager.VisualStateGroups>\r\n                <Path x:Name=\"Element\">\r\n                <Path.Fill>\r\n                    <RadialGradientBrush>\r\n                    <GradientStop Color=\"Yellow\" Offset=\"0\" />\r\n                    <GradientStop Color=\"Red\" Offset=\"1.5\" />\r\n                    </RadialGradientBrush>\r\n                </Path.Fill>\r\n                </Path>\r\n            </Grid>\r\n            </ControlTemplate>";
			if (durationSecond != 0)
			{
				string newValue = "00:00:" + durationSecond.ToString().PadLeft(2, '0');
				text = text.Replace("00:00:01", newValue);
			}
			text = text.Replace("Red", colorFill.ToString());
			text = text.Replace("Yellow", colorGlow.ToString());
			Stream stream = new MemoryStream(Encoding.Default.GetBytes(text));
			ControlTemplate controlTemplate = XamlReader.Load(stream) as ControlTemplate;
			return new FillSymbol
			{
				ControlTemplate = controlTemplate
			};
		}

		public static FillSymbol GetCustomFadeFillSymbol(Color colorFill, Color colorStroke, double strokeThickness, double normalOpacity, double mouseOverOpacity, int durationSecond)
		{
			string text = "<ControlTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'>\r\n            <Grid>\r\n              <VisualStateManager.VisualStateGroups>\r\n                <VisualStateGroup x:Name='CommonStates'>\r\n                  <VisualState x:Name='MouseOver'>\r\n                    <Storyboard>\r\n                      <DoubleAnimation BeginTime='0' Storyboard.TargetName='Element' Storyboard.TargetProperty='(Shape.Opacity)' To='0.75' Duration='00:00:01' />\r\n                    </Storyboard>\r\n                  </VisualState>\r\n                  <VisualState x:Name='Normal'>\r\n                    <Storyboard>\r\n                      <DoubleAnimation BeginTime='0' Storyboard.TargetName='Element' Storyboard.TargetProperty='(Shape.Opacity)' To='0.15' Duration='00:00:01' />\r\n                    </Storyboard>\r\n                  </VisualState>\r\n                </VisualStateGroup>\r\n              </VisualStateManager.VisualStateGroups>\r\n              <Path x:Name='Element' Fill='Magenta' Opacity='0.15' Stroke='Black' StrokeThickness='1'></Path>\r\n            </Grid>\r\n           </ControlTemplate>";
			if (durationSecond != 0)
			{
				string newValue = "00:00:" + durationSecond.ToString().PadLeft(2, '0');
				text = text.Replace("00:00:01", newValue);
			}
			text = text.Replace("Magenta", colorFill.ToString());
			text = text.Replace("Black", colorStroke.ToString());
			if (strokeThickness != 0.0)
			{
				string newValue2 = "StrokeThickness='" + strokeThickness.ToString() + "'";
				text = text.Replace("StrokeThickness='1'", newValue2);
			}
			if (normalOpacity != 0.0)
			{
				text = text.Replace("0.15", normalOpacity.ToString());
			}
			if (mouseOverOpacity != 0.0)
			{
				text = text.Replace("0.75", mouseOverOpacity.ToString());
			}
			Stream stream = new MemoryStream(Encoding.Default.GetBytes(text));
			ControlTemplate controlTemplate = XamlReader.Load(stream) as ControlTemplate;
			return new FillSymbol
			{
				ControlTemplate = controlTemplate
			};
		}
	}
}
