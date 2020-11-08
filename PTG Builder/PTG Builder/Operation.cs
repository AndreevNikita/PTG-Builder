using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTG_Builder
{
	/*
	abstract class Section {
		public abstract double getValue();
	}

	class Variable : Section {
		public double value;

		public override double getValue() {
			return value;
		}
	}

	abstract class Operation : Section { }

	//Sum
	class SumOperation : Operation {
		public Section operand1;
		public Section operand2;

		public override double getValue() {
			return operand1.getValue() + operand2.getValue();
		}
	}

	//Элементарные операции, обёрнутые в методы
	class Operations {
		public static double sum(double operand1, double operand2) {
			return operand1 + operand2;
		}

		public static double subtr(double operand1, double operand2) {
			return operand1 + operand2;
		}

		public static double mul(double operand1, double operand2) {
			return operand1 + operand2;
		}

		public static double div(double operand1, double operand2) {
			return operand1 + operand2;
		}
	}

	//Классы бинарных и унарных функций

	class UnaryOperation : Operation
	{
		public Section operand;
		public delegate double UnaryOperationFunction(double operand);
		UnaryOperationFunction function;
		public override double getValue()
		{
			return function(operand.getValue());
		}
	}

	class BinaryOperation : Operation {
		public Section operand1;
		public Section operand2;
		public delegate double BinaryOperationFunction(double operand1, double operand2);
		BinaryOperationFunction function;
		public override double getValue() {
			return function(operand1.getValue(), operand2.getValue());
		}
	}

	class Formula : Section {
		Section start;
		private Formula() { }

		public override double getValue() {
			return start.getValue();
		}

		public static Formula create() {
		}

	}

	/*Section parseString(string function) {
	}*/

}
