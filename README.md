Лабораторная работа №2: Разработка лексического анализатора (сканера)

Название: Разработка лексического анализатора (сканера) для языка программирования

Цель работы: Изучить назначение и принципы работы лексического анализатора в структуре компилятора. Спроектировать алгоритм (диаграмму состояний) и выполнить программную реализацию сканера для выделения лексем из входного текста. Интегрировать разработанный модуль в ранее созданный графический интерфейс языкового процессора.

Сведения об авторе
    Студент: Тарбаев Даба-Цырен
    Группа: АП-327

Постановка задачи
Разработать лексический анализатор (сканер) в соответствии с индивидуальным вариантом задания, интегрировать его в приложение из лабораторной работы №1 и обеспечить вывод результатов.

Требования к реализации сканера:

    Спроектировать диаграмму состояний конечного автомата

    Разработать программный модуль лексического анализа, который:

        принимает на вход строку (исходный текст программы)

        выделяет все допустимые лексемы согласно варианту

        классифицирует лексемы по типам

        обрабатывает недопустимые символы с указанием позиции

        учитывает многострочность входного текста

Вариант задания
Тема 4. Объявление комплексного числа с инициализацией на языке C#

Примеры корректных входных строк:

Complex c1 = new Complex(1.2, 6.0);
Complex var = new Complex(-3.5, -2);
Complex name = new Complex(10, -6.76);
Complex x = new Complex(-5, 3.14);

Допустимые лексемы:
Условный код	- Тип лексемы	- Лексемы
1	Ключевое слово Complex	
2	Ключевое слово new	
3	Идентификатор	c1, var, x, name
4	Оператор присваивания	=
5	Разделитель (пробел)	(пробел)
6	Оператор конструктора (	
7	Оператор конструктора )	
8	Знак минуса	-
9	Целое без знака	0, 42, 100
10	Вещественное число	1.2, 3.14, -6.76
11	Оператор перечисления	,
12	Оператор завершения	;
-1	Недопустимый символ	@, #, $ и т.д.


Диаграмма состояний
<img width="2404" height="3844" alt="Лаба 2 тфяк-Страница — 2" src="https://github.com/user-attachments/assets/5e3ad840-c47a-4cac-8d29-03a2adfe83f4" />
Рисунок 1 - Диаграмма состяний
Тестовые примеры
Представлен пример успешной обработки одной строки. (Рисунок 2) 
<img width="984" height="727" alt="изображение" src="https://github.com/user-attachments/assets/beb6e120-6f6d-4621-914d-39b04edad8b7" />
Рисунок 2 - Пример успешной обработки

Пример с недопустимыми символами в строке. (Рисунок 3) 
<img width="987" height="730" alt="изображение" src="https://github.com/user-attachments/assets/a21a6af4-dbd1-442b-8c37-cd3cf17c9156" />
Рисунок 3 - Пример с недопустимыми символами обработки

Многострочный пример. (Рисунок 4) 
<img width="983" height="729" alt="изображение" src="https://github.com/user-attachments/assets/37b7149c-b1b5-4ca5-9868-19422ebcd6ef" />
Рисунок 4 - Многострочный пример

Дополнительное задание

Были установлены FLEX&BISON и разработана грамматика для них. Результат прдеставлен на рисунке 5
<img width="980" height="725" alt="изображение" src="https://github.com/user-attachments/assets/5eea5728-0108-421d-a79e-5e26488e9895" />

Рисунок 5 - Успешно просканирвоанны строки

<img width="981" height="726" alt="изображение" src="https://github.com/user-attachments/assets/cf873c75-5676-4723-a01a-487adf89d9d6" />

Рисунок 6 - Ошибки

Файл program.l 
    
    %{
    #define YYSTYPE std::string
    #define YY_NO_UNISTD_H
    
    #include <string>
    #include "parser.tab.hpp"
    
    int line_number = 1;
    %}
    
    %option c++
    %option noyywrap
    %option yylineno
    
    %%
    
    "Complex" {
        yylval = yytext;
        return COMPLEX_TYPE;
    }
    
    "new" {
        yylval = yytext;
        return NEW;
    }
    
    "=" {
        yylval = yytext;
        return EQUALS;
    }
    
    ";" {
        yylval = yytext;
        return SEMICOLON;
    }
    
    "(" {
        yylval = yytext;
        return OPEN_PAREN;
    }
    
    ")" {
        yylval = yytext;
        return CLOSE_PAREN;
    }
    
    "," {
        yylval = yytext;
        return COMMA;
    }
    
    [a-zA-Z][a-zA-Z0-9]* {
        yylval = yytext;
        return ID;
    }
    
    [+-]?[0-9]+(\.[0-9]+)?([Ee][+-]?[0-9]+)? {
        yylval = yytext;
        return NUMBER;
    }
    
    \n {
        line_number = yylineno;
    }
    
    [ \t] {  }
    
    . {
        yylval = yytext;
        return LEXERROR;
    }
    
    %%

Файл program.y 
    
    %define parse.error verbose 
    %{
    #include <string>
    #include <list>
    #include "SyntaxError.h"
    
    extern int yylex();
    extern int line_number;
    
    std::list<syntaxError> error_list;
    
    void yyerror(const char* s)
    {
        syntaxError error;
        error.errorMessage = std::string("error: ") + s;
        error.line = line_number;
        error_list.push_back(error);           
    }
    %}
    
    %token COMPLEX_TYPE
    %token NEW
    %token ID
    %token EQUALS
    %token SEMICOLON
    %token OPEN_PAREN
    %token CLOSE_PAREN
    %token COMMA
    %token NUMBER
    %token LEXERROR
    
    %%
    
    prog: def progRem | ;
    
    def: COMPLEX_TYPE ID EQUALS NEW COMPLEX_TYPE OPEN_PAREN NUMBER COMMA NUMBER CLOSE_PAREN SEMICOLON
        | COMPLEX_TYPE error EQUALS NEW COMPLEX_TYPE OPEN_PAREN NUMBER COMMA NUMBER CLOSE_PAREN SEMICOLON
        | COMPLEX_TYPE ID error NEW COMPLEX_TYPE OPEN_PAREN NUMBER COMMA NUMBER CLOSE_PAREN SEMICOLON
        | COMPLEX_TYPE ID EQUALS error COMPLEX_TYPE OPEN_PAREN NUMBER COMMA NUMBER CLOSE_PAREN SEMICOLON
        | COMPLEX_TYPE ID EQUALS NEW error OPEN_PAREN NUMBER COMMA NUMBER CLOSE_PAREN SEMICOLON
        | COMPLEX_TYPE ID EQUALS NEW COMPLEX_TYPE error NUMBER COMMA NUMBER CLOSE_PAREN SEMICOLON
        | COMPLEX_TYPE ID EQUALS NEW COMPLEX_TYPE OPEN_PAREN error COMMA NUMBER CLOSE_PAREN SEMICOLON
        | COMPLEX_TYPE ID EQUALS NEW COMPLEX_TYPE OPEN_PAREN NUMBER error NUMBER CLOSE_PAREN SEMICOLON
        | COMPLEX_TYPE ID EQUALS NEW COMPLEX_TYPE OPEN_PAREN NUMBER COMMA error CLOSE_PAREN SEMICOLON
        | COMPLEX_TYPE ID EQUALS NEW COMPLEX_TYPE OPEN_PAREN NUMBER COMMA NUMBER error SEMICOLON
        | COMPLEX_TYPE ID EQUALS NEW COMPLEX_TYPE OPEN_PAREN NUMBER COMMA NUMBER CLOSE_PAREN error
        | LEXERROR SEMICOLON
        | error SEMICOLON;
    
    progRem: def progRem
           | /* empty */
           ;
    
    %%
