using NSE.Core.Utils;

namespace NSE.Core.DomainObjects
{
	public class CPF
	{
		public const int CpfMaxLength = 11;
		public string Numero { get; private set; }

		// Construtor do EntityFramework
		protected CPF() { }

		public CPF(string numero)
		{
			//ex
			if (!CPF.Validar(numero)) throw new DomainException("CPF inválido");

			Numero = numero;
		}

		public static bool Validar(string cpf)
		{
			cpf = cpf.ApenasNumeros(cpf);


			if (cpf.Length > 11)
				return false;

			while (cpf.Length != 11)
				cpf = '0' + cpf;

			var igual = true;

			for (int i = 0; i < 11; i++)
				if (cpf[i] != cpf[0])
					igual = false;

			if (igual || cpf == "12345678909")
				return false;

			var numeros = new int[11];

			for (int i = 0; i < 11; i++)
				numeros[i] = int.Parse(cpf[i].ToString());

			var soma = 0;
			for (int i = 0; i < 9; i++)
				soma += (10 - i) * numeros[i];

			var resultado = soma % 11;

			//if(resultado == 1 || resultado == 0)
			//{
			//	if (numeros[10] != 0)
			//		return false;
			//}
			//else if(numeros[10] != 11 - resultado)
			//	return false;

			return true;
		}

	}

}
