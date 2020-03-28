using System;
using System.Text;
using System.Collections.Generic;


class CsvReader {
  byte[] content;
  int    index;

  public CsvReader(byte[] content) {
    this.content = content;
    index = 0;
  }

  public void Skip(char ch) {
    if (!NextIs(ch))
      throw new Exception();
    Read();
  }

  public void SkipLine() {
    while (!Eof() && Read() != '\n')
      ;
  }

  public void NextLine() {
    SkipLine();
  }

  public List<object> ReadRow() {
    List<object> row = new List<object>();
    for ( ; ; ) {
      row.Add(ReadField());
      if (Eof())
        return row;
      if (NextIs('\n')) {
        Read();
        return row;
      }
      if (!NextIs(';')) {
        Console.WriteLine("Peek() = %s, row = %s", Peek(), row);
      }
      Check(NextIs(';'));
      Read();
    }
  }

  public object ReadField() {
    if (IsDigit(Peek()))
      return ReadNumber();
    else if (NextIs('"'))
      return ReadString();
    else
      throw new Exception();
  }

  public long ReadLong() {
    bool neg = NextIs('-');
    if (neg)
      Read();
    Check(IsDigit(Peek()));
    long value = Read() - '0';
    while (!Eof() && IsDigit(Peek()))
      value = 10 * value + Read() - '0';
    return neg ? -value : value;
  }

  public double ReadDouble() {
    bool neg = NextIs('-');
    if (neg)
      Read();
    Check(IsDigit(Peek()));
    double value = Read() - '0';
    while (!Eof() && IsDigit(Peek()))
      value = 10 * value + Read() - '0';
    if (Eof() || !NextIs('.'))
      return neg ? -value : value;
    Read();
    double weigth = 0.1;
    while (!Eof() && IsDigit(Peek())) {
      value += weigth * (Read() - '0');
      weigth = 0.1 * weigth;
    }
    return value;
  }

  public object ReadNumber() {
    bool neg = NextIs('-');
    if (neg)
      Read();
    Check(IsDigit(Peek()));
    long value = Read() - '0';
    while (!Eof() && IsDigit(Peek()))
      value = 10 * value + Read() - '0';
    if (Eof() || !NextIs('.'))
      return neg ? -value : value;
    Read();
    double floatValue = value;
    double digitValue = 0.1;
    while (!Eof() && IsDigit(Peek())) {
      floatValue += digitValue * (Read() - '0');
      digitValue = 0.1 * digitValue;
    }
    return floatValue;
  }

  public string ReadString() {
    StringBuilder sb = new StringBuilder();
    Check(NextIs('"'));
    Read();
    for ( ; ; ) {
      char ch = Read();
      if (ch == '"')
        if (!NextIs('"'))
          return sb.ToString();
        else
          Read();
      sb.Append(ch);
    }
  }

  char Read() {
    return (char) content[index++];
  }

  char Peek() {
    return (char) content[index];
  }

  bool NextIs(char ch) {
    return index < content.Length && content[index] == ch;
  }

  public bool Eof() {
    return index >= content.Length;
  }

  void Check(bool cond) {
    if (!cond)
      throw new Exception();
  }

  //////////////////////////////////////////////////////////////////////////////

  static bool IsDigit(char ch) {
    return ch >= '0' & ch <= '9';
  }
}
