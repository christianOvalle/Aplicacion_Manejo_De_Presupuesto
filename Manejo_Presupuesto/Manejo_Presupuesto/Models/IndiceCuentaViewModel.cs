namespace Manejo_Presupuesto.Models
{
    public class IndiceCuentaViewModel
    {
        public string TipoCuenta { get; set; }

        public IEnumerable<Cuenta> Cuentas { get; set; }

        public decimal Balance => Cuentas.Sum(x => x.Balance);
    }
}
