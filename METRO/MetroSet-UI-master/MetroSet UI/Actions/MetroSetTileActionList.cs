/**
 * MetroSet UI - MetroSet UI Framewrok
 * 
 * The MIT License (MIT)
 * Copyright (c) 2017 Narwin, https://github.com/N-a-r-w-i-n
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of 
 * this software and associated documentation files (the "Software"), to deal in the 
 * Software without restriction, including without limitation the rights to use, copy, 
 * modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
 * and to permit persons to whom the Software is furnished to do so, subject to the 
 * following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in 
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
 * PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
 * CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
 * OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using MetroSet_UI.Controls;
using MetroSet_UI.Design;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;

namespace MetroSet_UI.Tasks
{
    public class MetroSetTileActionList : DesignerActionList
    {
        private readonly MetroSetTile _metroSetTile;

        public MetroSetTileActionList(IComponent component) : base(component)
        {
            _metroSetTile = (MetroSetTile)component;
        }

        public Style Style
        {
            get => _metroSetTile.Style;
            set => _metroSetTile.Style = value;
        }

        public string ThemeAuthor => _metroSetTile.ThemeAuthor;

        public string ThemeName => _metroSetTile.ThemeName;

        public StyleManager StyleManager
        {
            get => _metroSetTile.StyleManager;
            set => _metroSetTile.StyleManager = value;
        }

        public string Text
        {
            get => _metroSetTile.Text;
            set => _metroSetTile.Text = value;
        }

        public Font Font
        {
            get => _metroSetTile.Font;
            set => _metroSetTile.Font = value;
        }

        public Image BackgroundImage
        {
            get => _metroSetTile.BackgroundImage;
            set => _metroSetTile.BackgroundImage = value;
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection items = new DesignerActionItemCollection
        {
            new DesignerActionHeaderItem("MetroSet Framework"),
            new DesignerActionPropertyItem("StyleManager", "StyleManager", "MetroSet Framework", "Gets or sets the stylemanager for the control."),
            new DesignerActionPropertyItem("Style", "Style", "MetroSet Framework", "Gets or sets the style."),

            new DesignerActionHeaderItem("Informations"),
            new DesignerActionPropertyItem("ThemeName", "ThemeName", "Informations", "Gets or sets the The Theme name associated with the theme."),
            new DesignerActionPropertyItem("ThemeAuthor", "ThemeAuthor", "Informations", "Gets or sets the The Author name associated with the theme."),

            new DesignerActionHeaderItem("Appearance"),
            new DesignerActionPropertyItem("Text", "Text", "Appearance", "Gets or sets the The text associated with the control."),
            new DesignerActionPropertyItem("Font", "Font", "Appearance", "Gets or sets the The font associated with the control."),
            new DesignerActionPropertyItem("BackgroundImage", "BackgroundImage", "Appearance", "Gets or sets the BackgroundImage associated with the control."),
        };

            return items;
        }
    }
}