CC := gcc
CFLAGS := -std=c11 -Wall -Wextra -pedantic
TARGET := kontejnery

.PHONY: all clean run

all: $(TARGET)

$(TARGET): main.c
	$(CC) $(CFLAGS) main.c -o $(TARGET)

run: $(TARGET)
	./$(TARGET)

clean:
	rm -f $(TARGET)
