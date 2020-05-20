#include <stdio.h>
#include <stdlib.h>

int f1(int a,int b)
{
    return a/b;
}
int f2(int a ,int b)
{
    return b/a;
}

int main()
{
int (*functii)(int,int);
functii=&f1;
printf("%d",functii(4,2));
functii=&f2;
printf("\n%d",functii(4,2));
}
