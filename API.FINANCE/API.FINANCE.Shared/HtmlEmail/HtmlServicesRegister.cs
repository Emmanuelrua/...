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
            var html = $"<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>FinancialGlow</title>\r\n    <style>\r\n        td {{\r\n            height: 10vh;\r\n            width: 80vw;\r\n            text-align: center;\r\n        }}\r\n\r\n        h2 {{\r\n            font-size: 3vw;\r\n            text-shadow: 4px 2px 3px rgba(179, 163, 116, 1);\r\n            margin-bottom: 15px;\r\n        }}\r\n\r\n        p {{\r\n            font-size: 1.5vw;\r\n        }}\r\n\r\n        .confirm-btn {{\r\n            width: 160px;\r\n            height: 80px;\r\n            border-radius: 15px;\r\n            font-size: 1.5em;\r\n            font-weight: 600;\r\n            border: 2.5px solid black;\r\n            background: linear-gradient(-45deg, rgba(179, 163, 116, 1), rgb(124, 107, 56));\r\n            box-shadow: 0 4px 20px 0 rgba(0, 0, 0, 0.5), 0 6px 20px 0 rgba(0, 0, 0, 0.5);\r\n            cursor: pointer;\r\n        }}\r\n\r\n        .confirm-btn a {{\r\n            text-decoration: none;\r\n            color: black;\r\n        }}\r\n\r\n        .confirm-btn:hover {{\r\n            transform: scale(1.2);\r\n        }}\r\n\r\n        @media (max-width: 635px) {{\r\n            .container {{\r\n                width: 112vw;\r\n                height: 110vh;\r\n            }}\r\n            .info {{\r\n                padding: 0;\r\n                margin: 0;\r\n            }}\r\n            td {{\r\n                height: 15vh;\r\n                text-align: center;\r\n            }}\r\n            h2 {{\r\n                width: 90vw;\r\n                font-size: 8vw;\r\n                margin: 10px;\r\n            }}\r\n\r\n            p {{\r\n                width: 100vw;\r\n                height: 10vh;\r\n                font-size: 4vw;\r\n                margin: 0;\r\n            }}\r\n            .image {{\r\n                width: 100px;\r\n                height: 100px;\r\n            }}\r\n        }}\r\n    </style>\r\n</head>\r\n<body>\r\n    <div class=\"container\">\r\n        <table class=\"info\">\r\n            <tbody>\r\n                <tr><td><img src=\"https://img.etimg.com/thumb/msid-59738997,width-640,resizemode-4,imgsize-21421/nike.jpg\" class=\"image\"></td></tr>\r\n                <tr><td><h2>Confirm your email</h2></td></tr>\r\n                <tr><td><p>We can't wait to see you at FinancialGlow!</p></td></tr>\r\n                <tr><td><p>To access your account and organize your finances, click here:</p></td></tr>\r\n                <tr><td><button class=\"confirm-btn\"><a href='{HtmlEncoder.Default.Encode(callbackURL)}'>Confirm</a></button></td></tr>\r\n            </tbody>\r\n        </table>\r\n    </div>\r\n</body>\r\n</html>";
            return html;
        }
    }
}
