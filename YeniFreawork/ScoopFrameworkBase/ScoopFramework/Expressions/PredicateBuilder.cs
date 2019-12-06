using System;
using System.Linq;
using System.Linq.Expressions;

namespace ScoopFramework.Expressions
{
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> False<T>() { return f => false; }
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
        }
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }
    }
}

/*
    LESSON 1
    var predicate = PredicateBuilder.True();
    predicate = predicate.And(x => x["Name"].Equals("Venkat"));
    predicate = predicate.And(x => x["Age"].Equals("26"));
    predicate.Compile();
    var filtered = dataTable.Where(predicate).ToList();

    *******************************************************************
    LESSON 2
    // initial "false" condition just to start "OR" clause with
    var predicate = PredicateBuilder.False<YourDataClass>();
    
    if (condition1)
    {
        predicate = predicate.Or(d => d.SomeStringProperty == "Tom");
    }
    
    if (condition2)
    {
        predicate = predicate.Or(d => d.SomeStringProperty == "Alex");
    }
    
    if (condition3)
    {
        predicate = predicate.And(d => d.SomeIntProperty >= 4);
    }
    
    return originalCollection.Where<YourDataClass>(predicate.Compile());

    ********************************************************************
    
    LESSON 3
    var searchPredicate = PredicateBuilder.False<Songs>();
    foreach(string str in strArray)
    {
       var closureVariable = str; // See the link below for the reason
       searchPredicate = 
         searchPredicate.Or(SongsVar => SongsVar.Tags.Contains(closureVariable));
    }
    
    var allSongMatches = db.Songs.Where(searchPredicate); 
    






*/
