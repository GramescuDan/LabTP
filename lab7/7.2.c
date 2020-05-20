#include <stdio.h>
#include <stdlib.h>
#include <math.h>

#ifdef IMPLICIT
double abs_real(double a)
{
    return fabs(a);
}
#endif // IMPLICIT


int main()
{
    double a;
    scanf("%lg",&a);
    printf("a=%lg si IMPLICIT=%lg =>%lg",a,IMPLICIT,abs_real(a));

}
