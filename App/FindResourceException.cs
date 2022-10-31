using System;

namespace App;

public class FindResourceException : Exception
{
    public FindResourceException(string msg) : base(msg)
    {
    }
}