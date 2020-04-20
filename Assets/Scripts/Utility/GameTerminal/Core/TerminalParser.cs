using System;
using System.Text.RegularExpressions;

namespace Utility.GameTerminal
{
    public class TerminalParser
    {
        const string m_operandPattern = @"[\s\S]*=";
        const string m_valuePattern = @"=[\s\S]*";

        public bool TryParse(string expression, out ParseResult result)
        {
            result = null;

            if(string.IsNullOrEmpty(expression))
            {
                return false;
            }
            string operand = string.Empty;
            string value = string.Empty;

            if (expression.Contains("="))
            {
                var operandMatch = Regex.Matches(expression, m_operandPattern, RegexOptions.Multiline);
                var valueMatch = Regex.Matches(expression, m_valuePattern, RegexOptions.Multiline);

                if (operandMatch.Count == 0)
                {
                    return false;
                }

                operand = operandMatch[0].Value.Replace("=", string.Empty);
                value = valueMatch[0].Value.Replace("=", string.Empty);
            }
            else
            {
                operand = expression;
            }
            
            result = new ParseResult(operand.Trim(), value.Trim());

            return true;
        }
    }
}
