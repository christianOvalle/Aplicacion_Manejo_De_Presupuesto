﻿using System.ComponentModel.DataAnnotations;

namespace Manejo_Presupuesto.Validaciones
{
    public class ValidandoPrimeraLetraAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var primeraLetra = value.ToString()[0].ToString();

            if (primeraLetra != primeraLetra.ToUpper()) {

                return new ValidationResult("La primera letra debe ser mayuscula");
            }

            return ValidationResult.Success;
        }

    }
}
