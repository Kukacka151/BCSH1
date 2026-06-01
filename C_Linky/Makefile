CC = gcc
CFLAGS = -std=c11 -Wall -Wextra -pedantic
TARGET = linky
OBJS = main.o linka.o

.PHONY: all run clean

all: $(TARGET)

$(TARGET): $(OBJS)
	$(CC) $(CFLAGS) -o $(TARGET) $(OBJS)

main.o: main.c linka.h
	$(CC) $(CFLAGS) -c main.c

linka.o: linka.c linka.h
	$(CC) $(CFLAGS) -c linka.c

run: all
	./$(TARGET)

clean:
	rm -f $(TARGET) $(OBJS)
