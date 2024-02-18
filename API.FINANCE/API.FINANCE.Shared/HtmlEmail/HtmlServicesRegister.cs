using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace API.FINANCE.Shared.HtmlEmail
{
    public class HtmlServicesRegister
    {
        public static string Estructure(string callbackURL)
        {
            var html = $"<!DOCTYPE html>\r\n<html lang=\"es\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Finance API</title>\r\n    <style>\r\n        * {{\r\n            margin: 0;\r\n            font-family: Cambria, Cochin, Georgia, Times, 'Times New Roman', serif;\r\n            background-color: beige;\r\n        }}\r\n\r\n        header {{\r\n            background-color: #efb810;\r\n            display: flex;\r\n            width: 100%;\r\n            height: 30%;\r\n            padding: 30px;\r\n            flex-direction: column;\r\n            justify-content: center;\r\n            font-size: large;\r\n            border-style: double;\r\n            border-width: 2px;\r\n            border-color: black;\r\n        }}\r\n\r\n        h1 {{\r\n            font-size: 2.5em;\r\n            margin-left: 12px;\r\n            margin-top: 10px;\r\n            background-color: #efb810;\r\n        }}\r\n\r\n        header p {{\r\n            margin-left: 50%;\r\n            margin-top: 30px;\r\n            font-size: x-large;\r\n            background-color: #efb810;\r\n        }}\r\n\r\n        .second img {{\r\n            width: 120px;\r\n            height: 120px;\r\n            box-shadow: 3px 3px 3px black;\r\n            border-radius: 10px;\r\n        }}\r\n\r\n        .second {{\r\n            width: 400px;\r\n            margin-left: 41%;\r\n            display: flex;\r\n            justify-content: space-evenly;\r\n            font-weight: bolder;\r\n            background-color: #efb810;\r\n        }}\r\n\r\n        .second h1 {{\r\n            padding-top: 20px;\r\n        }}\r\n\r\n        .s-part {{\r\n            display: flex;\r\n            flex-direction: column;\r\n            width: 40%;\r\n            height: 320px;\r\n            justify-content: center;\r\n            margin-left: 34%;\r\n            margin-top: 100px;\r\n            border-style: solid;\r\n            border-width: 2px;\r\n            border-color: rgba(0, 0, 0, 0.249);\r\n            border-radius: 50px;\r\n            font-size: 1.2em;\r\n        }}\r\n\r\n        .letter {{\r\n            display: flex;\r\n            align-items: center;\r\n            justify-content: center;\r\n        }}\r\n\r\n        .s-part button {{\r\n            width: 110px;\r\n            height: 40px;\r\n            margin-left: 42%;\r\n            background-color: #efb810;\r\n            border-radius: 10px;\r\n        }}\r\n        .s-part button a{{\r\n            background-color: #efb810;\r\n            color:black;\r\n            text-decoration: none;\r\n            font-size: x-large;\r\n\r\n        }}\r\n    </style>\r\n</head>\r\n<body>\r\n    <header>\r\n        <div class=\"second\">\r\n            <img src=\"https://imagenes.elpais.com/resizer/Wg0gq9RxxL8Hso8bRVdmTz1wqno=/980x0/cloudfront-eu-central-1.images.arcpublishing.com/prisa/MBAX3IRTQU5D3JD6N5YGCY6K6U.jpg\">\r\n            <h1>FINANCE API</h1>\r\n        </div>\r\n        <p>Medellín</p>\r\n    </header>\r\n\r\n    <section class=\"s-part\">\r\n        <p class=\"letter\">We can't wait to see you at FINANCE API!</p>\r\n        <br><br>\r\n        <p class=\"letter\">An application password has been created to log in to your account </p>\r\n        <br><br><br>\r\n        <button><a href='{HtmlEncoder.Default.Encode(callbackURL)}'>clicking here</a></button>\r\n    </section>\r\n</body>\r\n</html>";
            return html;
        }
    }
}
