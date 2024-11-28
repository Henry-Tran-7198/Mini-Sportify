#include <stdio.h>
#include <math.h> 
int main() 
{
    int xA;
    int yA;
    int xB;
    int yB;
    int xC;
    int yC;
    int xD;
    int yD;
    printf("Input xA, yA: ");
    scanf("%d", &xA);
    scanf("%d", &yA);
    printf("A(%d, %d)", xA, yA);
    printf("Input xB, yB: ");
    scanf("%d", &xB);
    scanf("%d", &yB);
    printf("B(%d, %d)", xB, yB);
    printf("Input xC, yC: ");
    scanf("%d", &xC);
    scanf("%d", &yC);
    printf("C(%d, %d)", xC, yC);
    printf("Input xD, yD: ");
    scanf("%d", &xD);
    scanf("%d", &yD);
    printf("D(%d, %d)", xD, yD);
    int AB;
    int AC;
    int BC;
    int AD;
    int BD;
    int CD;
    int pABD;
    int pBCD;
    int pACD;
    int pABC;
    int SABD;
    int SBCD;
    int SACD;
    int SABC;
    AB = sqrt((xB-xA)*(xB-xA)+(yB-yA)*(yB-yA));
    AC = sqrt((xC-xA)*(xC-xA)+(yC-yA)*(yC-yA));
    BC = sqrt((xC-xB)*(xC-xB)+(yC-yB)*(yC-yB));
    AD = sqrt((xD-xA)*(xD-xA)+(yD-yA)*(yD-yA));
    BD = sqrt((xD-xB)*(xD-xB)+(yD-yB)*(yD-yB));
    CD = sqrt((xD-xC)*(xD-xC)+(yD-yC)*(yD-yC));
    pABD = (AB + BD + AD) / 3;
    pBCD = (BC + CD + BD) / 3;
    pACD = (AC + CD + AD) / 3;
    pABC = (AB + BC + AC) / 3;
    SABD = sqrt(pABD*(pABD - AB)*(pABD - BD)*(pABD - AD));
    SBCD = sqrt(pBCD*(pBCD - BC)*(pBCD - CD)*(pBCD - BD));
    SACD = sqrt(pACD*(pACD - AC)*(pABC - CD)*(pABC - AD));
    SABC = sqrt(pABC*(pABC - AB)*(pABC - BC)*(pABC - AC));
    if (AB + BC > AB && AC + AB > BC){
        if(SABC == SABD + SACD + SBCD){
        printf("4 points A, B, C, D can create a ABC triangle having point D being inside of it.");
        }
        else{
            printf("4 points A, B, C, D can create a ABC triangle having point D being outside of it.");
        }
    }
    else{
        printf("3 points A, B, C can't create a triangle.");
    }
}