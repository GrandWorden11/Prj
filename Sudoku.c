#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>
void print_sudoku(int *sudoku[9][9])
{
    for (int i=0; i<9; i++)
    {
        for (int j=0; j<9; j++)
        {
            printf("cell at [%d][%d] is: %d" ,i,j, (*sudoku)[i][j]);
            printf("\n");
        }
    }
}
bool cube_check(int row, int col, int num, int sudoku[9][9])
{
    int row_pos = row/3;
    int col_pos = col/3;

    row_pos *= 3;
    col_pos *= 3;

    for(int i=0; i<3; i++)
    {
        for(int j=0; j<3; j++)
        {
            if (sudoku[row_pos+i,col_pos+j] == num)
            {
                return false;
            }
        }
    }
    return true;
}
bool row_check(int row, int col, int num, int sudoku[9][9])
{
    int row_pos = row/3;
    int col_pos = col/3;

    row_pos *= 3;
    col_pos *= 3;

    if(row_pos == 0)
    {
        for(int i=0; i<9; i++)
        {
            if (sudoku[row_pos+i,col_pos] == num)
            {
                return false;
            }
        }
    }
    else if(row_pos == 8)
    {
        for(int i=0; i<=8; i++)
        {
            if (sudoku[row_pos-i,col_pos] == num)
            {
                return false;
            }
        }
    }
    else
    {
        for(int i = row_pos; i<9; i++)
        {
            if (sudoku[row_pos+i,col_pos] == num)
            {
                return false;
            }
        }
        for(int i=0; i<row_pos; i++)
        {
            if (sudoku[row_pos-i,col_pos] == num)
            {
                return false;
            }
        }
    }
    return true;
}
bool col_check(int row, int col, int num, int sudoku[9][9])
{
    int row_pos = row/3;
    int col_pos = col/3;

    row_pos *= 3;
    col_pos *= 3;

    if(col_pos == 0)
    {
        for(int j=0; j<9; j++)
        {
            if (sudoku[row_pos,col_pos+j] == num)
            {
                return false;
            }
        }
    }
    else if(col_pos == 8)
    {
        for(int j=0; j<9; j++)
        {
            if (sudoku[row_pos,col_pos-j] == num)
            {
                return false;
            }
        }
    }
    else
    {
        for(int j = 0; j<9; j++)
        {
            if (sudoku[row_pos,col_pos+j] == num)
            {
                return false;
            }
        }
        for(int j = 0; j<col_pos; j++)
        {
            if (sudoku[row_pos,col_pos-j] == num)
            {
                return false;
            }
        }
    }
    return true;
}
void solve_sudoku(int *sudoku[9][9])
{
    for (int i=0; i<9; i++)
    {
        for (int j=0; j<9; j++)
        {
            if(sudoku[i][j] == 0)
            {
                if(cube_check(i,j,j,*sudoku) == true && row_check(i,j,j,*sudoku) == true && col_check(i,j,j,*sudoku) == true)
                {
                    (*sudoku)[i][j] = j;
                }
            }
        }
    }
    return sudoku;
}
bool is_solved(int *sudoku[9][9])
{
    for(int i=0; i<9; i++)
    {
        for(int j=0; j<9; j++)
        {
            if(sudoku[i][j] == 0)
            {
                return false;
            }
        }
    }
    return true;
}
int main()
{
    int sudoku[9][9];
    for (int i=0; i<9; i++)
    {
        for (int j=0; j<9; j++)
        {
            printf("Enter a cell [%d][%d](0 for null):",i,j);
            scanf("%d", &sudoku[i][j]);
        }
    }
    print_sudoku(&sudoku);
    solve_sudoku(&sudoku);
    print_sudoku(&sudoku);

    return 0;
}
