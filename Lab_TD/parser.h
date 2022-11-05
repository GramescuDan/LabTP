#ifndef _PARSER_AT__
#define _PARSER_AT__

#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define AT_COMMAND_MAX_LINES 100
#define AT_COMMAND_MAX_LINE_SIZE 100
typedef struct
{
    char data[AT_COMMAND_MAX_LINES][AT_COMMAND_MAX_LINE_SIZE + 1];

}AT_COMMAND_DATA;

extern AT_COMMAND_DATA mydata;

typedef enum
{
    STATE_MACHINE_NOT_READY,
    STATE_MACHINE_READY_OK,
    STATE_MACHINE_READY_WITH_ERROR
}STATE_MACHINE_RETURN_VALUE;

void funBegin(char *filename);

#endif