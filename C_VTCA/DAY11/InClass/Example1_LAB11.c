#include <stdio.h>
int main(int argc, char const *argv[])
{
    FILE *f;
    f = fopen("Example1_LAB11.c", "r");
    if (f != NULL){
        char ch;
        while ((ch=fgetc(f))!=-1){
            putchar(ch);
        }
        fclose(f);

    }
    else{
        printf("Can't read text file.");
    }
    return 0;
}
