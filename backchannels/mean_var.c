#include <stdlib.h>
#include "m_pd.h"

static t_class *mean_var_class;

typedef struct {
  t_object  x_obj;
  t_outlet *mean_out;
  t_outlet *var_out;
} MeanVar;

void mean_var_calc(MeanVar *x,
                   t_symbol *s,
                   int argc,
                   t_atom *argv)
{
  (void)(s);  // Suppress "unused variable" warning
  size_t n = argc;
  t_float x_mean = 0;
  t_float x_squared_mean = 0;
  for (size_t i = 0; i < n; i++)
  {
    float val = atom_getfloat(argv + i);
    x_mean += val;
    x_squared_mean += (val * val);
  }
  x_mean /= n;
  x_squared_mean /= n;

  outlet_float(x->mean_out, x_mean);
  outlet_float(x->var_out, x_squared_mean - x_mean * x_mean);
}

void *mean_var_new()
{
  MeanVar *x = (MeanVar *) pd_new(mean_var_class);
  x->mean_out = outlet_new(&x->x_obj, &s_float);
  x->var_out = outlet_new(&x->x_obj, &s_float);
  return (void *) x;
}

void mean_var_setup(void) {
  mean_var_class = class_new(gensym("mean_var"),
                             (t_newmethod) mean_var_new,
                             0,
                             sizeof(MeanVar),
                             CLASS_DEFAULT,
                             0);

  class_addlist(mean_var_class, mean_var_calc);
}
