//***************************************************************************
//
//    Copyright (c) Microsoft Corporation. All rights reserved.
//    This code is licensed under the Visual Studio SDK license terms.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//***************************************************************************

using System;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace Outline.TodoGlyph
{
    /// <summary>
    /// This class implements IGlyphFactory, which provides the visual
    /// element that will appear in the glyph margin.
    /// </summary>
    internal class ToDoGlyphFactory : IGlyphFactory
    {
        private const double _width = 10.0;
        private const double _height = 10.0;
        private IAdornmentLayer _layer;
        private ITextSelection _selection;
        public ToDoGlyphFactory(IWpfTextView view)
        {
            this._layer = view.GetAdornmentLayer("SelectionHighlight");
            this._selection = view.Selection;
        }

        public UIElement GenerateGlyph(IWpfTextViewLine line, IGlyphTag tag)
        {
            if (tag == null || !(tag is ToDoTag))
            {
                return null;
            }

            ToDoTag tags = tag as ToDoTag;
            if (tags.HightText == "todo")
            {
                return new Ellipse { 
                    Fill=Brushes.Orange,
                    StrokeThickness=1.2,
                    Stroke = Brushes.OrangeRed,
                    Height = 12.0,
                    Width = 12.0
                };
            }
            else
            {
                return new Rectangle
                {
                    //Fill = new SolidColorBrush(Colors.GreenYellow),
                    Fill = new SolidColorBrush(Colors.DarkOrchid),
                    StrokeThickness = 1.0,
                    Stroke = Brushes.Black,
                    Height = 10.0,
                    Width = 10.0
                };
            }
        }

    }
}
