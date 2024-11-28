#include <stdio.h>
int main (){
    char ch;
    printf("Input a letter (from a to z): ");
    scanf("%c", &ch);
    if (ch < 'a' || ch > 'z'){
        printf("The letter you typed is not in the alphabet. \n");
    }
    else{
        switch (ch)
        {
            case 'a': case 'e': case 'u': case 'i': case 'o':
            printf("The letter you typed is a vowel. \n");
            break;
            case 'z':
            printf("The letter you typed is a final letter in the alphabet. \n");
            break;
            default:
            printf("The letter you typed is a consonant \n");
            break;
        }
    }
}