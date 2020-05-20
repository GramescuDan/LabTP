#include <stdio.h>
#include <stdlib.h>

union mare{
char peste_zero;
char sub_zero;
};

typedef struct numere{
int nr;
union mare lit_2;
};

void afisare (struct numere *p)
{
    switch(p->nr)
case '1':
    {
        printf("%c",p.lit_2.peste_zero);
        break;
    }
case '-1'
    {
       printf("%c",p.lit_2.sub_zero);
        break;
    }
}
void citire (struct numere *p)
{

}

int main()
{
    struct numere p;
    citire(&p);
    afisare(&p);
}
