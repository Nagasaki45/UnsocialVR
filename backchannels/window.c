#include <stdlib.h>
#include "m_pd.h"

#define MAX_WINDOW_SIZE 8192

static t_class *window_class;

typedef struct {
  t_object  x_obj;
  t_outlet *out;
  size_t window_size;
  size_t tail;
  t_atom window[2 * MAX_WINDOW_SIZE];
} Window;

void window_calc(Window *x, t_floatarg new_value)
{
  // When reaching the end of the buffer rotate
  if (x->tail + x->window_size >= (MAX_WINDOW_SIZE * 2))
  {
    for(size_t i = 0; i < x->window_size; i++)
    {
      x->window[i] = *(x->window + x->tail + i);
    }
    x->tail = 0;
  }

  // Insert the new value and return
  SETFLOAT(x->window + x->tail + x->window_size, new_value);
  x->tail++;
  outlet_list(x->out, &s_list, x->window_size, x->window + x->tail);
}

void *window_new(t_floatarg window_size)
{
  if (window_size > MAX_WINDOW_SIZE)
  {
    error("window: window_size out of range, using %d", MAX_WINDOW_SIZE);
    window_size = MAX_WINDOW_SIZE;
  }

  Window *x = (Window *) pd_new(window_class);

  x->window_size = window_size;
  x->tail = 0;

  for(size_t i = 0; i < x->window_size; i++)
  {
    SETFLOAT(x->window + i, 0.0f);
  }

  x->out = outlet_new(&x->x_obj, &s_list);

  return (void *) x;
}

void window_setup(void) {
  window_class = class_new(gensym("window"),
                           (t_newmethod) window_new,
                           0,
                           sizeof(Window),
                           CLASS_DEFAULT,
                           A_DEFFLOAT,  // window_size
                           0);

  class_addfloat(window_class, window_calc);
}
