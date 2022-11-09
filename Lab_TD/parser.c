
#include "parser.h"

AT_COMMAND_DATA mydata;
static uint32_t state = 0;
static uint32_t line = 0;

void addCharInLine(const char current_character){
    char aux = current_character;
strncat(mydata.data[line],&aux,1);
}

void cleanMyData(){
    for(size_t i = 0; i < 1000; ++i){
        for(size_t j = 0; j <100; ++j){
            mydata.data[i][j] = '\0';
        }
    }

}

STATE_MACHINE_RETURN_VALUE parseNextChar(unsigned char current_character)
{
    switch (state)
    {
        case 0:
        {   cleanMyData();
            printf("state 0: %x \n",current_character);
            if (current_character == '\r')
            {
                state = 1;
            }
            break;
        }
        case 1:
        {
            printf("state 0: %x \n",current_character);
            if (current_character == '\n')
            {
                state = 2;
            }
            else
            {
                return STATE_MACHINE_READY_WITH_ERROR;
            }
            break;
        }
        case 2:
        {printf("state 2: %c\n",current_character);
            if (current_character == 'O')
            {
                state = 3;
            }
            else if (current_character == 'E')
            {
                state = 7;
            }
            else if (current_character == '+')
            {
                state = 14;
            }

            else
            {
                return STATE_MACHINE_READY_WITH_ERROR;
            }
            break;
        }
        case 3:
        {   printf("state 3: %c \n",current_character);
            if (current_character == 'K')
            {
                state = 4;
            }
            else
            {

                return STATE_MACHINE_READY_WITH_ERROR;
            }
            break;
        }

        case 4:
        {
            printf("state 4: %x\n",current_character);
            if (current_character == 0x0D)
            {
                state = 5;
            }
            else
            {
                return STATE_MACHINE_READY_WITH_ERROR;
            }
            break;
        }

        case 5:
        {printf("state 5: %x\n",current_character);
            if (current_character == 0x0A)
            {
                state = 0;
                return STATE_MACHINE_READY_OK;
            }
            else
            {
                return STATE_MACHINE_READY_WITH_ERROR;
            }
        }

        case 7:
        {
            printf("state 7: %c\n",current_character);
            if (current_character == 'R')
            {
                state = 8;
            }
            else
            {
                return STATE_MACHINE_READY_WITH_ERROR;
            }
            break;
        }

        case 8:
        {
            printf("state 8: %c\n",current_character);
            if (current_character == 'R')
            {
                state = 9;
            }
            else
            {
                return STATE_MACHINE_READY_WITH_ERROR;
            }
            break;
        }

        case 9:
        {
            printf("state 9: %c\n",current_character);
            if (current_character == 'O')
            {
                state = 10;
            }
            else
            {
                return STATE_MACHINE_READY_WITH_ERROR;
            }
            break;
        }

        case 10:
        {
            printf("state 10: %c\n",current_character);
            if (current_character == 'R')
            {
                state = 11;
            }
            else
            {
                return STATE_MACHINE_READY_WITH_ERROR;
            }
            break;
        }

        case 11:
        {
            printf("state 11: %c\n",current_character);
            if (current_character == 0x0D)
            {
                state = 12;
            }
            else
            {
                return STATE_MACHINE_READY_WITH_ERROR;
            }
            break;
        }

        case 12:
        {
            if (current_character == 0x0A)
            {
                return STATE_MACHINE_READY_OK;
            }
            else
            {
                return STATE_MACHINE_READY_WITH_ERROR;
            }
        }

        case 14:
        {

            printf("state 14: %c\n",current_character);
            if (current_character >= 0x20 && current_character < 0x7F)
            {
                //ADD CHAR
                addCharInLine(current_character);
                state = 15;
            }
            else
            {
                return STATE_MACHINE_READY_WITH_ERROR;
            }
            break;
        }

        case 15:
        {
            printf("state 15: %c\n",current_character);
            if (current_character == 0X0D)
            {
                state = 16;
            }
            else if (current_character >= 0x20 && current_character < 0x7F)
            {
                //ADD CHAR
                addCharInLine(current_character);
                state = 15;
            }

            else
            {
                return STATE_MACHINE_READY_WITH_ERROR;
            }
            break;
        }

        case 16:
        {
            if (current_character == 0x0A)
            {
                state = 17;
                printf("%c",current_character);
            }
            else
            {
                return STATE_MACHINE_READY_WITH_ERROR;
            }
            break;
        }

        case 17:
        {
            if (current_character == 0x0D)
            {
                state = 18;
                printf("%c",current_character);
            }
            else if(current_character == '+')
            {
                //ENDLINE
                line++;
                state = 14;
                printf("%c",current_character);
            }

            else
            {
                return STATE_MACHINE_READY_WITH_ERROR;
            }
            break;
        }

        case 18:
        {
            if (current_character == 0x0A)
            {
                state = 19;
                printf("%c",current_character);
            }
            else
            {
                return STATE_MACHINE_READY_WITH_ERROR;
            }
            break;
        }

        case 19:
        {
            if (current_character == 'O')
            {
                state = 3;
                printf("%c",current_character);
            }
            else if (current_character == 'E')
            {
                state = 7;
                printf("%c",current_character);
            }
            else
            {
                return STATE_MACHINE_READY_WITH_ERROR;
            }
            break;
        }

    }
    return STATE_MACHINE_NOT_READY;
}