#include <stdio.h>
#include <string.h>
struct Books{
    char isbn[15];
    char title[51];
    char author[51];
    float price;
};
void printBook(struct Books book);
struct Books getBook();
void getString(char *str, int length);
void printLine();
void printTitle();
int main(){
    struct Books books[] =
    {
      {"978-0131103627", "The C Progamming Language", "Dennis M. Ritchie", 52.89}
    };
    int i, count = 3;
    printf("Input ")
}