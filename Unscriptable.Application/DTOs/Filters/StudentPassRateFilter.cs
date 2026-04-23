using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unscriptable.Application.DTOs.Filters;

public class StudentPassRateFilter
{
    //[Range(1, int.MaxValue, ErrorMessage = "StudentId должен быть больше 0")]
    public int StudentId { get; set; }
}
