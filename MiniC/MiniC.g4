grammar MiniC;

// ||| Parser //

compileUnit	: (statement|functionDefinition)+
			;

statement	: expression SEMICOLON			#StatementExpression
			| conditionStatement			#StatementCondition
			| repetitionStatement			#StatementRepetition
			| BREAK SEMICOLON				#StatementBreak
			| RETURN expression SEMICOLON	#StatementReturn
			| compoundStatement				#StatementCompound
			;

functionDefinition	: FUNCTION IDENTIFIER LEFT_PARENTHESIS formalArguments? RIGHT_PARENTHESIS compoundStatement
					;

expression	: NUMBER														#ExpressionNUMBER
			| IDENTIFIER													#ExpressionIDENTIFIER
			| IDENTIFIER LEFT_PARENTHESIS actualArguments RIGHT_PARENTHESIS	#ExpressionFunctionCall
			| LEFT_PARENTHESIS expression RIGHT_PARENTHESIS					#ExpressionParenthesis
			| NOT expression												#ExpressionLogicalNot
			| expression AND expression										#ExpressionLogicalAnd
			| expression OR expression										#ExpressionLogicalOr
			| expression operator=(EQUAL|NEQUAL|GT|GTE|LT|LTE) expression	#ExpressionLogicalComparative
			| expression operator=(MUL|DIV) expression						#ExpressionMultiplicative
			| expression operator=(PLUS|MINUS) expression					#ExpressionAdditive
			| PLUS expression												#ExpressionPositive
			| MINUS expression												#ExpressionNegative
			| IDENTIFIER ASSIGNMENT expression								#ExpressionAssignment
			;

conditionStatement	: IF LEFT_PARENTHESIS expression RIGHT_PARENTHESIS statement (ELSE statement)?
					;

repetitionStatement	: WHILE LEFT_PARENTHESIS expression RIGHT_PARENTHESIS compoundStatement
					;

compoundStatement	: LEFT_CURLY_BRACKET RIGHT_CURLY_BRACKET				#CompoundStatementEmpty
					| LEFT_CURLY_BRACKET (statement)+ RIGHT_CURLY_BRACKET	#CompoundStatementNotEmpty
					;

actualArguments	: (expression (COMMA)?)+
				;

formalArguments	: (IDENTIFIER (COMMA)?)+
				;

// ||| Lexer //

IF : 'if' ;
ELSE : 'else' ;
WHILE : 'while' ;
BREAK : 'break' ;
FUNCTION : 'function' ;
RETURN : 'return' ; 

COMMA : ','; 
SEMICOLON : ';';

ASSIGNMENT : '=' ;
PLUS : '+' ;
MINUS : '-' ;
MUL : '*' ;
DIV : '/' ;

NOT : '!' ;
OR : '||' ;
AND : '&&' ;
EQUAL : '==' ;
NEQUAL : '!=' ; 
GT : '>' ;
LT : '<' ;
GTE : '>=' ;
LTE : '<=' ;

LEFT_PARENTHESIS : '(' ;
RIGHT_PARENTHESIS : ')' ;
LEFT_CURLY_BRACKET : '{' ;
RIGHT_CURLY_BRACKET : '}' ;

NUMBER : '0'|[1-9][0-9]*;
IDENTIFIER : [a-zA-Z][a-zA-Z0-9_]*;

WS : [ \t\n\r]+ -> skip ;
