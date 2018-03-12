#include <stdlib.h>
#include "m_pd.h"

static t_class *linear_regression_class;

typedef struct {
  t_object  x_obj;
  t_outlet *b_out, *a_out;
} LinearRegression;

void linear_regression_calc(LinearRegression *linear_regression,
                            t_symbol *s,
                            int argc,
                            t_atom *argv)
{
  // Simple linear regression calculation based on wiki
  // https://en.wikipedia.org/wiki/Simple_linear_regression
  (void)(s);  // Suppress "unused variable" warning
  size_t n = argc;
  t_float sx = 0;
  t_float sy = 0;
  t_float sxx = 0;
  t_float sxy = 0;
  t_float syy = 0;
  for (size_t i = 0; i < n; i++)
  {
    t_float x = i;
    t_float y = atom_getfloat(argv + i);
    sx += x;
    sy += y;
    sxx += (x * x);
    sxy += (x * y);
    syy += (y * y);
  }
  float b = (n * sxy - sx * sy) / (n * sxx - sx * sx);
  float a = (sy - b * sx) / n;
  outlet_float(linear_regression->b_out, b);
  outlet_float(linear_regression->a_out, a);
}

void *linear_regression_new()
{
  LinearRegression *x = (LinearRegression *) pd_new(linear_regression_class);
  x->b_out = outlet_new(&x->x_obj, &s_float);
  x->a_out = outlet_new(&x->x_obj, &s_float);
  return (void *) x;
}

void linear_regression_setup(void) {
  linear_regression_class = class_new(gensym("linear_regression"),
                                      (t_newmethod) linear_regression_new,
                                      0,
                                      sizeof(LinearRegression),
                                      CLASS_DEFAULT,
                                      0);

  class_addlist(linear_regression_class, linear_regression_calc);
}
