using OSMtoSharp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Factories.Helpers
{
    public class OsmToUnityConverter
    {
        public static Vector2 GetPointFromUnityPointVec2(Point point, OsmBounds bounds)
        {
            float x = (((float)point.Lat - (float)bounds.MinLat) * Assets.Scripts.Constants.Constants.multipler);
            float y = -(((float)point.Lon - (float)bounds.MinLon) * Assets.Scripts.Constants.Constants.multipler);

            return new Vector2(x, y);
        }

        public static Vector3 GetPointFromUnityPointVec3(Point point, OsmBounds bounds)
        {
            float x = (((float)point.Lat - (float)bounds.MinLat) * Assets.Scripts.Constants.Constants.multipler);
            float y = 0f;
            float z = -(((float)point.Lon - (float)bounds.MinLon) * Assets.Scripts.Constants.Constants.multipler);

            return new Vector3(x, y, z);
        }
        public static string ColorToHex(Color32 color)
        {
            string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
            return hex;
        }

        public static Color HexToColor(string hex)
        {
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            return new Color32(r, g, b, 255);
        }


        public static bool OnlyHexInString(string test)
        {
            // For C-style hex notation (0xFF) you can use @"\A\b(0[xX])?[0-9a-fA-F]+\b\Z"
            return System.Text.RegularExpressions.Regex.IsMatch(test, @"\A\b[0-9a-fA-F]+\b\Z");
        }
        public static string StringColorToHexString(string colorString)
        {
            switch (colorString)
            {
                case "black": return "#000000";
                case "navy": return "#000080";
                case "darkblue": return "#00008B";
                case "mediumblue": return "#0000CD";
                case "blue": return "#0000FF";
                case "darkgreen": return "#006400";
                case "green": return "#008000";
                case "teal": return "#008080";
                case "darkcyan": return "#008B8B";
                case "deepskyblue": return "#00BFFF";
                case "darkturquoise": return "#00CED1";
                case "mediumspringgreen": return "#00FA9A";
                case "lime": return "#00FF00";
                case "springgreen": return "#00FF7F";
                case "aqua": return "#00FFFF";
                case "cyan": return "#00FFFF";
                case "midnightblue": return "#191970";
                case "dodgerblue": return "#1E90FF";
                case "lightseagreen": return "#20B2AA";
                case "forestgreen": return "#228B22";
                case "seagreen": return "#2E8B57";
                case "darkslategray": return "#2F4F4F";
                case "limegreen": return "#32CD32";
                case "mediumseagreen": return "#3CB371";
                case "turquoise": return "#40E0D0";
                case "royalblue": return "#4169E1";
                case "steelblue": return "#4682B4";
                case "darkslateblue": return "#483D8B";
                case "mediumturquoise": return "#48D1CC";
                case "indigo ": return "#4B0082";
                case "darkolivegreen": return "#556B2F";
                case "cadetblue": return "#5F9EA0";
                case "cornflowerblue": return "#6495ED";
                case "rebeccapurple": return "#663399";
                case "mediumaquamarine": return "#66CDAA";
                case "dimgray": return "#696969";
                case "dimgrey": return "#696969";
                case "slateblue": return "#6A5ACD";
                case "olivedrab": return "#6B8E23";
                case "slategray": return "#708090";
                case "slategrey": return "#708090";
                case "lightslategray": return "#778899";
                case "lightslategrey": return "#778899";
                case "mediumslateblue": return "#7B68EE";
                case "lawngreen": return "#7CFC00";
                case "chartreuse": return "#7FFF00";
                case "aquamarine": return "#7FFFD4";
                case "maroon": return "#800000";
                case "purple": return "#800080";
                case "olive": return "#808000";
                case "gray": return "#808080";
                case "grey": return "#808080";
                case "skyblue": return "#87CEEB";
                case "lightskyblue": return "#87CEFA";
                case "blueviolet": return "#8A2BE2";
                case "darkred": return "#8B0000";
                case "darkmagenta": return "#8B008B";
                case "saddlebrown": return "#8B4513";
                case "darkseagreen": return "#8FBC8F";
                case "lightgreen": return "#90EE90";
                case "mediumpurple": return "#9370DB";
                case "darkviolet": return "#9400D3";
                case "palegreen": return "#98FB98";
                case "darkorchid": return "#9932CC";
                case "yellowgreen": return "#9ACD32";
                case "sienna": return "#A0522D";
                case "brown": return "#A52A2A";
                case "darkgray": return "#A9A9A9";
                case "darkgrey": return "#A9A9A9";
                case "lightblue": return "#ADD8E6";
                case "greenyellow": return "#ADFF2F";
                case "paleturquoise": return "#AFEEEE";
                case "lightsteelblue": return "#B0C4DE";
                case "powderblue": return "#B0E0E6";
                case "firebrick": return "#B22222";
                case "darkgoldenrod": return "#B8860B";
                case "mediumorchid": return "#BA55D3";
                case "rosybrown": return "#BC8F8F";
                case "darkkhaki": return "#BDB76B";
                case "silver": return "#C0C0C0";
                case "mediumvioletred": return "#C71585";
                case "indianred ": return "#CD5C5C";
                case "peru": return "#CD853F";
                case "chocolate": return "#D2691E";
                case "tan": return "#D2B48C";
                case "lightgray": return "#D3D3D3";
                case "lightgrey": return "#D3D3D3";
                case "thistle": return "#D8BFD8";
                case "orchid": return "#DA70D6";
                case "goldenrod": return "#DAA520";
                case "palevioletred": return "#DB7093";
                case "crimson": return "#DC143C";
                case "gainsboro": return "#DCDCDC";
                case "plum": return "#DDA0DD";
                case "burlywood": return "#DEB887";
                case "lightcyan": return "#E0FFFF";
                case "lavender": return "#E6E6FA";
                case "darksalmon": return "#E9967A";
                case "violet": return "#EE82EE";
                case "palegoldenrod": return "#EEE8AA";
                case "lightcoral": return "#F08080";
                case "khaki": return "#F0E68C";
                case "aliceblue": return "#F0F8FF";
                case "honeydew": return "#F0FFF0";
                case "azure": return "#F0FFFF";
                case "sandybrown": return "#F4A460";
                case "wheat": return "#F5DEB3";
                case "beige": return "#F5F5DC";
                case "whitesmoke": return "#F5F5F5";
                case "mintcream": return "#F5FFFA";
                case "ghostwhite": return "#F8F8FF";
                case "salmon": return "#FA8072";
                case "antiquewhite": return "#FAEBD7";
                case "linen": return "#FAF0E6";
                case "lightgoldenrodyellow": return "#FAFAD2";
                case "oldlace": return "#FDF5E6";
                case "red": return "#FF0000";
                case "fuchsia": return "#FF00FF";
                case "magenta": return "#FF00FF";
                case "deeppink": return "#FF1493";
                case "orangered": return "#FF4500";
                case "tomato": return "#FF6347";
                case "hotpink": return "#FF69B4";
                case "coral": return "#FF7F50";
                case "darkorange": return "#FF8C00";
                case "lightsalmon": return "#FFA07A";
                case "orange": return "#FFA500";
                case "lightpink": return "#FFB6C1";
                case "pink": return "#FFC0CB";
                case "gold": return "#FFD700";
                case "peachpuff": return "#FFDAB9";
                case "navajowhite": return "#FFDEAD";
                case "moccasin": return "#FFE4B5";
                case "bisque": return "#FFE4C4";
                case "mistyrose": return "#FFE4E1";
                case "blanchedalmond": return "#FFEBCD";
                case "papayawhip": return "#FFEFD5";
                case "lavenderblush": return "#FFF0F5";
                case "seashell": return "#FFF5EE";
                case "cornsilk": return "#FFF8DC";
                case "lemonchiffon": return "#FFFACD";
                case "floralwhite": return "#FFFAF0";
                case "snow": return "#FFFAFA";
                case "yellow": return "#FFFF00";
                case "lightyellow": return "#FFFFE0";
                case "ivory": return "#FFFFF0";
                case "white": return "#FFFFFF";


                default:
                    return string.Empty;
            }
        }

    }
}
