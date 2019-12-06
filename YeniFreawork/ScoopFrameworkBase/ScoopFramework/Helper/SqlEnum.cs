namespace ScoopFramework.Helper
{
    class SqlEnum
    {
    }
    public enum QueryOrderType
    {
        ASC,
        DESC,
    }

    public enum BinaryOperator
    {
        And,
        New,
        Or,
        Not,
        Coalesce,
        Equal,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
        NotEqual,
        Like,
        AndAlso,
        OrElse,
        In,
        IsNull,
        IsNotNull,
    }
    public enum TransformOperator
    {
        Add,
        New,
        Divide,
        Modulo,
        Multiply,
        Negate,
        Power,
        Subtract,
        Lambda,
        Conditional,
        OnesComplement,
        ExclusiveOr,
    }

    public enum QueryFunctions
    {

        // String Functions
        Ascii,
        Char,
        CharIndex,
        Concat,
        Difference,
        Format,
        Left,
        Len,
        Lower,
        Ltrim,
        Nchar,
        Patindex,
        Quotename,
        Replace,
        Replicate,
        Reverse,
        Right,
        Rtrim,
        Trim,
        Soundex,
        Space,
        Str,
        String_Escape,
        String_Split,
        Stuff,
        Substring,
        Unicode,
        Upper,

        // Math Functions
        Abs,
        Acos,
        Asin,
        Atan,
        Atn2,
        Ceiling,
        Cos,
        Cot,
        Degrees,
        Exp,
        Floor,
        Log,
        PI,
        Power,
        Radians,
        Rand,
        Round,
        Sign,
        Sin,
        Sqrt,
        Square,
        Tan,

        // Datetime Functions
        GetDate,

     
        // Aggregate Functions
        Avg,
        Checksum_Agg,
        Count,
        Count_Big,
        Grouping,
        Grouping_Id,
        Max,
        Min,
        Sum,
        Stdev,
        Stdevp,
        Var,
        Varp,
    }
}
