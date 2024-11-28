#include <stdio.h>
#include <string.h>
struct Books{
    char isbn[15];
    char title[50];
    char author[50];
    float price;
};
int main(){
    struct Books book1;
    struct Books book2;
    strcpy(book1.isbn, "978-0131103627");
    strcpy(book1.title, "The C Progamming Language");
    strcpy(book1.author, "Dennis M. Ritchie");
    book1.price = 52.89;
    strcpy(book2.isbn, "978-0789751980");
    strcpy(book2.title, "C Progamming Absolute Beginner's Guide");
    strcpy(book2.author, "Dean Miller");
    book2.price = 24.32;
    printf("Book 1 isbn: %s \n", book1.isbn);
    printf("Book 1 title: %s \n", book1.title);
    printf("Book 1 author: %s \n", book1.author);
    printf("Book 1 price: %f \n", book1.price);
    printf("Book 2 isbn: %s \n", book2.isbn);
    printf("Book 2 title: %s \n", book2.title);
    printf("Book 2 author: %s \n", book2.author);
    printf("Book 2 price: %f \n", book2.price);
}