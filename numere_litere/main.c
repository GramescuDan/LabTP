#include <stdio.h>
// tip de bancnota sau moneda
typedef struct{
int val; // valoarea in bani
char *nume;
}Tip;
#define NTIPURI 27
#define ALTTIP 9
Tip tipuri[NTIPURI]={{1000,"o mie "},
{900,"noua sute "},{800,"opt sute "},
{700,"sapte sute "},{600,"sase sute "},
{500,"cinci sute "},{400,"patru sute "},
{300,"trei sute "},{200,"doua sute "},
{100,"o suta "},{90,"noua zeci "},
{80,"opt zeci "},{70,"sapte zeci "},
{60,"saizeci "},{50,"cinci zeci "},
{40,"patru zeci "},{30,"trei zeci "},{20,"doua zeci "},
{10,"zece "},{9,"noua "},{8,"opt "},{7,"sapte "},{6,"sase "},{5,"cinci "},{4,"patru "},{3,"trei "},{2,"doi "},{1,"unu "}};

Tip alttip[ALTTIP]={{19,"nouasprezece "},{18,"optisprezece "},{17,"saptisprezece "},{16,"saisprezece "},{15,"cincisprezece "},{14,"paisprezece "},
{13,"treisprezece "},{12,"doisprezece "},{11,"unsprezece "}};

int main()
{
int numar;
printf("valoare: ");scanf("%d",&numar);
if(numar==0)
    {printf("zero");
    }
int alTip=0;
if((numar<20)&&(numar>10))
{
    while(numar)
    {
        if(numar-alttip[alTip].val==0)
            printf("%s",alttip[alTip].nume);
            alTip++;
    }
}


int iTip=0; // indexul curent in tipuri
while(numar){ // cat timp mai sunt bani de platit
int n=numar/tipuri[iTip].val;
if(n){
printf("%s",tipuri[iTip].nume);
if((tipuri[iTip].val>=10)&&(tipuri[iTip].val<=90))
printf("si ");
numar-=n*tipuri[iTip].val;
}
++iTip;
}
return 0;
}
