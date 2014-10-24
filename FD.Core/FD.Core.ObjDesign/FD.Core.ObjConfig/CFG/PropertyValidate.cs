using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STComponse.CFG
{
    public class PropertyValidate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
//                VersionMan
                String checkName = value.ToString();

                return checkName.Length < 12 ? ValidationResult.Success : new ValidationResult("用户名长度不能超过12");
                //return checkName == "jv9" ? ValidationResult.Success : new ValidationResult("请使用指定用户名");
            }
            else
            {
                return new ValidationResult("用户名不能为空");
            }
        }
        
    }
}
