import java.util.List;
import java.util.ArrayList;


public class CsvReader {
  byte[] content;
  int    index;

  public CsvReader(byte[] content) {
    this.content = content;
    index = 0;
  }

  public void skip(char ch) {
    if (!nextIs(ch))
      throw new RuntimeException();
    read();
  }

  public void skipLine() {
    while (!eof() && read() != '\n')
      ;
  }

  public List<Object> readRow() {
    List<Object> row = new ArrayList<Object>();
    for ( ; ; ) {
      row.add(readField());
      if (eof())
        return row;
      if (nextIs('\n')) {
        read();
        return row;
      }
      if (!nextIs(';')) {
        System.out.printf("peek() = %s, row = %s\n", peek(), row);
      }
      check(nextIs(';'));
      read();
    }
  }

  Object readField() {
    if (Character.isDigit(peek()))
      return readNumber();
    else if (nextIs('"'))
      return readString();
    else
      throw new RuntimeException();
  }

  long readLong() {
    boolean neg = nextIs('-');
    if (neg)
      read();
    check(Character.isDigit(peek()));
    long value = read() - '0';
    while (!eof() && Character.isDigit(peek()))
      value = 10 * value + read() - '0';
    return neg ? -value : value;
  }

  Double readDouble() {
    boolean neg = nextIs('-');
    if (neg)
      read();
    check(Character.isDigit(peek()));
    double value = read() - '0';
    while (!eof() && Character.isDigit(peek()))
      value = 10 * value + read() - '0';
    if (eof() || !nextIs('.'))
      return neg ? -value : value;
    read();
    double weigth = 0.1;
    while (!eof() && Character.isDigit(peek())) {
      value += weigth * (read() - '0');
      weigth = 0.1 * weigth;
    }
    return value;
  }

  Number readNumber() {
    boolean neg = nextIs('-');
    if (neg)
      read();
    check(Character.isDigit(peek()));
    long value = read() - '0';
    while (!eof() && Character.isDigit(peek()))
      value = 10 * value + read() - '0';
    if (eof() || !nextIs('.'))
      return neg ? -value : value;
    read();
    double floatValue = value;
    double digitValue = 0.1;
    while (!eof() && Character.isDigit(peek())) {
      floatValue += digitValue * (read() - '0');
      digitValue = 0.1 * digitValue;
    }
    return floatValue;
  }

  String readString() {
    StringBuilder sb = new StringBuilder();
    check(nextIs('"'));
    read();
    for ( ; ; ) {
      char ch = read();
      if (ch == '"')
        if (!nextIs('"'))
          return sb.toString();
        else
          read();
      sb.append(ch);
    }
  }

  char read() {
    return (char) content[index++];
  }

  char peek() {
    return (char) content[index];
  }

  boolean nextIs(char ch) {
    return index < content.length && content[index] == ch;
  }

  boolean eof() {
    return index >= content.length;
  }

  void check(boolean cond) {
    if (!cond)
      throw new RuntimeException();
  }
}
